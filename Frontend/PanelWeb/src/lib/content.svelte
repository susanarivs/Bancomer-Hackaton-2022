<script>
    import Prospectos from './prospectos.svelte';
    import Change from './change.svelte'
</script>

<div id="content">
    <div class="blankbox">

    </div>
    <Prospectos></Prospectos>

    <Change></Change>
    <div class="blankbox">

    </div>
</div>

<style>
    #content{
        width: 100%;
        flex: 9;
        display: flex;
        column-gap: 3%;
        height: 100%;
    }
    .blankbox{
        flex: 1;
    }
</style>