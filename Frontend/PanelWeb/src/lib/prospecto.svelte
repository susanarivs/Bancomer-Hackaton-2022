<script>
    export let name
    export let curp
    export let telefono
    export let mail
    let color = "white"
    let selected = false
    function select(){
        if(selected){
            color = "white"
            selected = false
        }
        else if(!selected){
            color = "#ff5766"
            selected = true
        }
    }
</script>

<tr id="prospecto" style="background-color: {color}" on:click={select}>
    <td class="cell">{name}</td>
    <td class="cell">{curp}</td>
    <td class="cell">{telefono}</td>
    <td class="cell">{mail}</td>
</tr>

<style>
    .cell{
        width: 25%;
        //flex: 0;
    }
    #prospecto{
        width: 100%;
        //flex: 1;
        font-family: Arial, Helvetica, sans-serif;
        font-size: 1rem;
        line-height: 300%;
    }
    td {
        border: 1px solid black;
        border-collapse: collapse;
    }
</style>