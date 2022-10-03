# Importar los paquetes necesarios
import numpy as np
import cv2


def translate(image, x, y):
    # Definir la matriz de traslado
    M = np.float32([[1, 0, x], [0, 1, y]])
    shifted = cv2.warpAffine(image, M, (image.shape[1], image.shape[0]))

    # Devolver la imagen trasladada
    return shifted


def rotate(image, angle, center=None, scale=1.0):
    # Toma las dimensiones de la imagen.
    (h, w) = image.shape[:2]

    # Si el centro es None, inicialícelo como el centro de la imagen.
    if center is None:
        center = (w / 2, h / 2)

    # Realiza la rotación
    M = cv2.getRotationMatrix2D(center, angle, scale)
    rotated = cv2.warpAffine(image, M, (w, h))

    # Devolver la imagen rotada
    return rotated


def resize(image, width=None, height=None, inter=cv2.INTER_AREA):
    # inicializar las dimensiones de la imagen para cambiar 
    # el tamaño y tomar el tamaño de la imagen
    dim = None
    (h, w) = image.shape[:2]

    # si tanto el ancho como la altura son None, 
    # devolver la imagen original
    if width is None and height is None:
        return image

    # verificar si el ancho es None
    if width is None:
        # calcular la altura y construir las dimensiones
        r = height / float(h)
        dim = (int(w * r), height)

    # de lo contrario, la altura es None
    else:
        # calcular la relación del ancho y construir las dimensiones
        r = width / float(w)
        dim = (width, int(h * r))

    # cambiar el tamaño de la imagen
    resized = cv2.resize(image, dim, interpolation=inter)

    # devolver la imagen redimensionada
    return resized
