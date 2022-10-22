<script>
   export let lista = []
   import Prospecto from './prospecto.svelte'

</script>

<tbody id="lista" >

    {#each lista as l}
        <Prospecto
                name="{l.Nombres+' '+l.ApellidoPaterno+' '+l.ApellidoMaterno}"
                curp="{l.Curp}"
                telefono="{l.Telefono}"
                mail="{l.CorreoElectronico}"
        >

        </Prospecto>
    {/each}
</tbody>

<style>
    #lista{
        display: block;
        width: 100%;

        height: 100%;
        /*flex: 100;

        flex-direction: column;*/
        overflow: auto;

    }

</style>