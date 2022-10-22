<script>
    import TabHeader from './tabHeader.svelte'
    import {activeList} from "../stores.js"
    import {prospectos} from "../stores.js"
    import {porEnviar} from "../stores.js"
    import {enviados} from "../stores.js"
    import {respuestas} from "../stores.js"
    import PanelControl from "./panelControl.svelte"
    import Lista from "./lista.svelte"
    let listas = ['Por enviar', 'Enviados', 'Respuestas']
    let porEnviarA = []
    let enviadosA = []
    let respuestasA = []
    let prospectosA = [porEnviarA,enviadosA,respuestasA]
    let listIndex;
    let url = "http://localhost/bbva/API/selectFrom.php"
    let porEnviarPromise = fetch(url)
        .then((response) => {
            return response.json()
        })
        .then((data) => {
            return data
        })

    function addTo(array,value) {
        let ar = []
        if(array === 0){

        }
    }
    activeList.subscribe(value => {
        listIndex = value;
    })
    prospectos.subscribe(value => {
        prospectosA = value
    })
</script>

<table id="prospectos">
    <TabHeader></TabHeader>
    {#await porEnviarPromise then porEnviarA}
        <Lista lista="{porEnviarA}"></Lista>
    {/await}
    <PanelControl></PanelControl>
</table>

<style>
    #prospectos{
        display: block;
        width: 100%;
        flex: 15;

        //background-color: #09f;
    }
    table{
        border: 1px solid black;
        border-collapse: collapse;
    }
</style>