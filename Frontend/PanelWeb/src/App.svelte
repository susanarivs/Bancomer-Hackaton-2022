<script>
    import Upmenu from './lib/upmenu.svelte';
    import Content from './lib/content.svelte';
</script>

<main>
    <Upmenu></Upmenu>
    <Content></Content>
</main>

<style>
  main{
      display: flex;
      flex-direction: column;
      height: 100%;
  }
</style>
