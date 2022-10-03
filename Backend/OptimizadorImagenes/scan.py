# COMO USAR:
# python scan.py (--images <IMG_DIR> | --image <IMG_PATH>) [-i]
# Por ejemplo, para escanear una sola impagen en modo interactivo:
# python scan.py --image sample_images/desk.JPG -i
# Para escanear todas las imágenes en un directorio automáticamente:
# python scan.py --images sample_images

# El resultado se va colocar en un directorio llamado 'output'

from pyimagesearch import transform
from pyimagesearch import imutils
from scipy.spatial import distance as dist
from matplotlib.patches import Polygon
import numpy as np
import matplotlib.pyplot as plt
import itertools
import math
import cv2.cv2
from pylsd.lsd import lsd

import argparse
import os
import numpy as np
import namegenerator


class DocScanner(object):
    """Escáner de imágenes"""

    def __init__(self, interactive=False, MIN_QUAD_AREA_RATIO=0.25, MAX_QUAD_ANGLE_RANGE=40):
        """
        Args:
            interactivo (boolean): si es verdadero, el usuario puede ajustar el contorno 
            de la pantalla antes de que se produzca la transformación en la ventana de 
            pyplot interactivo.

            MIN_QUAD_AREA_RATIO (float): Se rechazará un contorno si sus esquinas no 
            forman un cuadrilátero que cubra al menos MIN_QUAD_AREA_RATIO de la imagen 
            original. El valor predeterminado es 0,25.

            MAX_QUAD_ANGLE_RANGE (int): Un contorno también será rechazado si el rango de 
            sus ángulos interiores excede MAX_QUAD_ANGLE_RANGE. El valor predeterminado es 40.
        """
        self.interactive = interactive
        self.MIN_QUAD_AREA_RATIO = MIN_QUAD_AREA_RATIO
        self.MAX_QUAD_ANGLE_RANGE = MAX_QUAD_ANGLE_RANGE

    def filter_corners(self, corners, min_dist=20):
        """Filtra esquinas que están dentro de min_dist de otros"""
        def predicate(representatives, corner):
            return all(dist.euclidean(representative, corner) >= min_dist
                       for representative in representatives)

        filtered_corners = []
        for c in corners:
            if predicate(filtered_corners, c):
                filtered_corners.append(c)
        return filtered_corners

    def angle_between_vectors_degrees(self, u, v):
        """Devuelve el ángulo entre dos vectores en grados"""
        return np.degrees(
            math.acos(np.dot(u, v) / (np.linalg.norm(u) * np.linalg.norm(v))))

    def get_angle(self, p1, p2, p3):
        """
        Devuelve el ángulo entre el segmento de línea de p2 a p1 
        y el segmento de línea de p2 a p3 en grados
        """
        a = np.radians(np.array(p1))
        b = np.radians(np.array(p2))
        c = np.radians(np.array(p3))

        avec = a - b
        cvec = c - b

        return self.angle_between_vectors_degrees(avec, cvec)

    def angle_range(self, quad):
        """
        Devuelve el rango entre los ángulos interiores máximo y mínimo del cuadrilátero. 
        El cuadrilátero de entrada debe ser una matriz numpy con vértices ordenados en 
        el sentido de las agujas del reloj comenzando con el vértice superior izquierdo.
        """
        tl, tr, br, bl = quad
        ura = self.get_angle(tl[0], tr[0], br[0])
        ula = self.get_angle(bl[0], tl[0], tr[0])
        lra = self.get_angle(tr[0], br[0], bl[0])
        lla = self.get_angle(br[0], bl[0], tl[0])

        angles = [ura, ula, lra, lla]
        return np.ptp(angles)

    def get_corners(self, img):
        """
        Devuelve una lista de esquinas ((x, y) tuplas) encontradas en la imagen de entrada. 
        Con el preprocesamiento y filtrado adecuados, debería generar como máximo 10 esquinas 
        potenciales. Esta es una función de utilidad utilizada por get_contours. Se espera 
        que la imagen de entrada se vuelva a escalar y Canny filtre antes de pasarla.
        """
        lines = lsd(img)

        corners = []
        if lines is not None:
            # separe las líneas horizontales y verticales, y volverlas a dibujar en lienzos separados
            lines = lines.squeeze().astype(np.int32).tolist()
            horizontal_lines_canvas = np.zeros(img.shape, dtype=np.uint8)
            vertical_lines_canvas = np.zeros(img.shape, dtype=np.uint8)
            for line in lines:
                x1, y1, x2, y2, _ = line
                if abs(x2 - x1) > abs(y2 - y1):
                    (x1, y1), (x2, y2) = sorted(
                        ((x1, y1), (x2, y2)), key=lambda pt: pt[0])
                    cv2.line(horizontal_lines_canvas, (max(x1 - 5, 0), y1),
                             (min(x2 + 5, img.shape[1] - 1), y2), 255, 2)
                else:
                    (x1, y1), (x2, y2) = sorted(
                        ((x1, y1), (x2, y2)), key=lambda pt: pt[1])
                    cv2.line(vertical_lines_canvas, (x1, max(y1 - 5, 0)),
                             (x2, min(y2 + 5, img.shape[0] - 1)), 255, 2)

            lines = []

            # encontrar las líneas horizontales (componentes conectados -> cuadros delimitadores -> líneas finales)
            (contours, hierarchy) = cv2.findContours(
                horizontal_lines_canvas, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
            contours = sorted(contours, key=lambda c: cv2.arcLength(
                c, True), reverse=True)[:2]
            horizontal_lines_canvas = np.zeros(img.shape, dtype=np.uint8)
            for contour in contours:
                contour = contour.reshape((contour.shape[0], contour.shape[2]))
                min_x = np.amin(contour[:, 0], axis=0) + 2
                max_x = np.amax(contour[:, 0], axis=0) - 2
                left_y = int(np.average(contour[contour[:, 0] == min_x][:, 1]))
                right_y = int(np.average(
                    contour[contour[:, 0] == max_x][:, 1]))
                lines.append((min_x, left_y, max_x, right_y))
                cv2.line(horizontal_lines_canvas,
                         (min_x, left_y), (max_x, right_y), 1, 1)
                corners.append((min_x, left_y))
                corners.append((max_x, right_y))

            # encontrar las líneas verticales (componentes conectados -> cuadros delimitadores -> líneas finales)
            (contours, hierarchy) = cv2.findContours(
                vertical_lines_canvas, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
            contours = sorted(contours, key=lambda c: cv2.arcLength(
                c, True), reverse=True)[:2]
            vertical_lines_canvas = np.zeros(img.shape, dtype=np.uint8)
            for contour in contours:
                contour = contour.reshape((contour.shape[0], contour.shape[2]))
                min_y = np.amin(contour[:, 1], axis=0) + 2
                max_y = np.amax(contour[:, 1], axis=0) - 2
                top_x = int(np.average(contour[contour[:, 1] == min_y][:, 0]))
                bottom_x = int(np.average(
                    contour[contour[:, 1] == max_y][:, 0]))
                lines.append((top_x, min_y, bottom_x, max_y))
                cv2.line(vertical_lines_canvas, (top_x, min_y),
                         (bottom_x, max_y), 1, 1)
                corners.append((top_x, min_y))
                corners.append((bottom_x, max_y))

            # encontrar las esquinas
            corners_y, corners_x = np.where(
                horizontal_lines_canvas + vertical_lines_canvas == 2)
            corners += zip(corners_x, corners_y)

        # eliminar las esquinas proximas
        corners = self.filter_corners(corners)
        return corners

    def is_valid_contour(self, cnt, IM_WIDTH, IM_HEIGHT):
        """Devuelve True si el contorno satisface todos los requisitos establecidos al crear la instancia"""

        return (len(cnt) == 4 and cv2.contourArea(cnt) > IM_WIDTH * IM_HEIGHT * self.MIN_QUAD_AREA_RATIO
                and self.angle_range(cnt) < self.MAX_QUAD_ANGLE_RANGE)

    def get_contour(self, rescaled_image):
        """
        Devuelve una matriz numérica de forma (4, 2) que contiene los vértices de las cuatro esquinas 
        del documento en la imagen. Considera las esquinas devueltas por get_corners() y utiliza la 
        heurística para elegir las cuatro esquinas que probablemente representen las esquinas del documento. 
        Si no se encontraron esquinas, o las cuatro esquinas representan un cuadrilátero que es demasiado 
        pequeño o convexo, devuelve las cuatro esquinas originales.
        """

        # estas constantes se eligen cuidadosamente
        MORPH = 9
        CANNY = 84
        HOUGH = 25

        IM_HEIGHT, IM_WIDTH, _ = rescaled_image.shape

        # convertir la imagen a escala de grises y desenfocarla ligeramente
        gray = cv2.cvtColor(rescaled_image, cv2.COLOR_BGR2GRAY)
        gray = cv2.GaussianBlur(gray, (7, 7), 0)

        # dilatar ayuda a eliminar posibles agujeros entre los segmentos del borde
        kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (MORPH, MORPH))
        dilated = cv2.morphologyEx(gray, cv2.MORPH_CLOSE, kernel)

        # buscar bordes y marcarlos en el mapa de salida usando el algoritmo Canny
        edged = cv2.Canny(dilated, 0, CANNY)
        test_corners = self.get_corners(edged)

        approx_contours = []

        if len(test_corners) >= 4:
            quads = []

            for quad in itertools.combinations(test_corners, 4):
                points = np.array(quad)
                points = transform.order_points(points)
                points = np.array([[p] for p in points], dtype="int32")
                quads.append(points)

            # obtener los cinco cuadriláteros principales por área
            quads = sorted(quads, key=cv2.contourArea, reverse=True)[:5]
            # ordenar los cuadriláteros candidatos por su rango de ángulos, lo que ayuda a eliminar los valores atípicos
            quads = sorted(quads, key=self.angle_range)

            approx = quads[0]
            if self.is_valid_contour(approx, IM_WIDTH, IM_HEIGHT):
                approx_contours.append(approx)

            # descomentar el código para dibujar las esquinas y el contorno encontrado
            # por get_corners() y ponerlo en la imagen

            # cv2.drawContours(rescaled_image, [approx], -1, (20, 20, 255), 2)
            # plt.scatter(*zip(*test_corners))
            # plt.imshow(rescaled_image)
            # plt.show()

        # también intentar encontrar contornos directamente desde la imagen con bordes, lo que a veces produce mejores resultados
        (cnts, hierarchy) = cv2.findContours(
            edged.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        cnts = sorted(cnts, key=cv2.contourArea, reverse=True)[:5]

        # bucle sobre los contornos
        for c in cnts:
            # aproximar el contorno
            approx = cv2.approxPolyDP(c, 80, True)
            if self.is_valid_contour(approx, IM_WIDTH, IM_HEIGHT):
                approx_contours.append(approx)
                break

        # Si no encontramos ningún contorno válido, simplemente usamos la imagen completa
        if not approx_contours:
            TOP_RIGHT = (IM_WIDTH, 0)
            BOTTOM_RIGHT = (IM_WIDTH, IM_HEIGHT)
            BOTTOM_LEFT = (0, IM_HEIGHT)
            TOP_LEFT = (0, 0)
            screenCnt = np.array(
                [[TOP_RIGHT], [BOTTOM_RIGHT], [BOTTOM_LEFT], [TOP_LEFT]])

        else:
            screenCnt = max(approx_contours, key=cv2.contourArea)

        return screenCnt.reshape(4, 2)

    def scan(self, image_path):

        RESCALED_HEIGHT = 500.0
        OUTPUT_DIR = 'output'

        # cargar la imagen y calcular la proporción de la altura
        # anterior a la nueva altura, clonarla y cambiar su tamaño
        image = cv2.imread(image_path)

        assert (image is not None)

        ratio = image.shape[0] / RESCALED_HEIGHT
        orig = image.copy()
        rescaled_image = imutils.resize(image, height=int(RESCALED_HEIGHT))

        # obtener el contorno del documento
        screenCnt = self.get_contour(rescaled_image)

        # aplicar la transformación de perspectiva
        warped = transform.four_point_transform(orig, screenCnt * ratio)

        # convertir la imagen deformada a escala de grises
        gray = cv2.cvtColor(warped, cv2.COLOR_BGR2GRAY)

        # agudizar la imagen
        sharpen = cv2.GaussianBlur(gray, (0, 0), 3)
        sharpen = cv2.addWeighted(gray, 1.5, sharpen, -0.5, 0)

        # aplicar un umbral adaptativo para obtener un efecto de blanco y negro
        thresh = cv2.adaptiveThreshold(
            sharpen, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 21, 15)

        # guardar la imagen transformada
        basename = os.path.basename(image_path)
        cv2.imwrite(OUTPUT_DIR + '/' + basename, thresh)
        print("imagen procesada en> " + (OUTPUT_DIR + '/' + basename))

    def scan64(self, b64_bytes):

        RESCALED_HEIGHT = 500.0
        OUTPUT_DIR = 'output'

        # cargar la imagen y calcular la proporción de la altura
        # anterior a la nueva altura, clonarla y cambiar su tamaño
        fbt = np.asarray(bytearray(b64_bytes.read()), dtype=np.uint8)
        image = cv2.imdecode(fbt, cv2.IMREAD_COLOR)

        assert (image is not None)

        ratio = image.shape[0] / RESCALED_HEIGHT
        orig = image.copy()
        rescaled_image = imutils.resize(image, height=int(RESCALED_HEIGHT))

        # obtener el contorno del documento
        screenCnt = self.get_contour(rescaled_image)

        # aplicar la transformación de perspectiva
        warped = transform.four_point_transform(orig, screenCnt * ratio)

        # convertir la imagen deformada a escala de grises
        gray = cv2.cvtColor(warped, cv2.COLOR_BGR2GRAY)

        # agudizar la imagen
        sharpen = cv2.GaussianBlur(gray, (0, 0), 3)
        sharpen = cv2.addWeighted(gray, 1.5, sharpen, -0.5, 0)

        # aplicar un umbral adaptativo para obtener un efecto de blanco y negro
        thresh = cv2.adaptiveThreshold(
            sharpen, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 21, 15)

        # guardar la imagen transformada
        resultimg = OUTPUT_DIR + '/' + namegenerator.gen() + ".jpg"
        cv2.imwrite(resultimg, thresh)
        return resultimg


if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    group = ap.add_mutually_exclusive_group(required=True)
    group.add_argument("--images", help="Directorio de imágenes a escanear")
    group.add_argument(
        "--image", help="Ruta a la imagen única que se va a escanear")
    ap.add_argument("-i", action='store_true',
                    help="Bandera para verificar y/o configurar manualmente las esquinas del documento")

    args = vars(ap.parse_args())
    im_dir = args["images"]
    im_file_path = args["image"]
    interactive_mode = args["i"]

    scanner = DocScanner(interactive_mode)

    valid_formats = [".jpg", ".jpeg", ".jp2", ".png", ".bmp", ".tiff", ".tif"]

    def get_ext(f):
        return os.path.splitext(f)[1].lower()

    # Escanear una sola imagen especificada por el argumento
    # de la línea de comando --image <IMAGE_PATH>
    if im_file_path:
        scanner.scan(im_file_path)

    # Escanee todas las imágenes válidas en el directorio especificado
    # por el argumento de la línea de comando --images <IMAGE_DIR>
    else:
        im_files = [f for f in os.listdir(im_dir) if get_ext(f) in valid_formats]
        for im in im_files:
            scanner.scan(im_dir + '/' + im)
