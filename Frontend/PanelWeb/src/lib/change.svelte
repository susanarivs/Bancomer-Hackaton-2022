<script>
    import {activeList} from "../stores.js"

    let textAbove = "Enviados"
    let textBellow = "Respuestas"
    let stateAbove = 0
    let stateBellow = 0
    function changeText(box) {
        if(box === 1){
            if(stateAbove === 0){
                activeList.update(n => 1)
                stateBellow = 0
                textBellow = "Respuestas"
                stateAbove = 1
                textAbove = "Por enviar"
            }
            else if(stateAbove === 1){
                activeList.update(n => 0)
                console.log('Aqui estoy')
                stateAbove = 0
                textAbove = "Enviados"
            }
        }
        if(box === 2){
            if(stateBellow === 0){
                activeList.update(n => 2)
                stateBellow = 1
                stateAbove = 0
                textAbove = "Enviados"
                textBellow = "Por enviar"
            }
            else if(stateBellow === 1){
                activeList.update(n => 0)
                console.log('Aqui estoy')
                stateBellow = 0
                textBellow = "Respuestas"
            }
        }
    }
</script>

<div id="change">
    <button id="above" class="box" on:click={() => changeText(1)}>
        {textAbove}
    </button>
    <button id="bellow" class="box" on:click={() => changeText(2)}>
        {textBellow}
    </button>
</div>

<style>
    .box{
        flex: 1;
        background-color: #0099ff;
    }
    #change{
        height: 100%;
        flex: 3;
        display: flex;
        flex-direction: column;
        row-gap: 10%;
    }
    .blankbox{
        flex: 1;
    }
</style>