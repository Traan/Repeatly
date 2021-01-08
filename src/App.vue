<template>
    <ul id="tree">
        <TreeItem class="item" :item="treeData" @make-folder="makeFolder" @add-item="addItem"></TreeItem>
    </ul>
    <button v-on:click="save">Save</button>
</template>

<script>
import TreeItem from './components/TreeItem.vue'

var data = {
                name: "My Tree",
                children: [
                    { 
                        name: "hello123" 
                    },
                    { 
                        name: "wat" 
                    },
                    {
                        name: "child folder",
                        children: [
                            {
                                name: "child folder",
                                children: [
                                    { name: "hello" }, 
                                    { name: "wat" }
                                ]
                            },
                            { 
                                name: "hello" 
                            },
                            { 
                                name: "wat" 
                            },
                            {
                                name: "child folder",
                                children: [
                                    { name: "hello" }, 
                                    { name: "wat" }
                                ]
                            }
                        ]
                    }
                ]
            };

export default {
    name: 'App',
    components: {
        TreeItem
    },
    data: function() {
        return {
            treeData: data
        }
    },
    methods: {
        makeFolder: function(item) {
            item.children = [];
            this.addItem(item);
        },
        addItem: function(item) {
            item.children.push({
                name: "new stuff"
            });
        },
        save: function() {            
            console.log(this.treeData);
        }
    }
}
</script>

<style>
body {
    font-family: Menlo, Consolas, monospace;
    color: #444;
}
ul {
    padding-left: 1em;
    line-height: 1.5em;
    list-style-type: dot;
}
</style>
