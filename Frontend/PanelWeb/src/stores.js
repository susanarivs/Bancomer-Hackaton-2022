import { writable } from 'svelte/store';
export const activeList = writable(0);
export const prospectos = writable("prospectos")
export const porEnviar = writable("porEnviar")
export const enviados = writable("enviados")
export const respuestas = writable("respuestas")