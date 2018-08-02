<template>
    <b-row>
        <b-col md="5">  
            <div>Options</div>          
            <b-form-select v-model="toAdd" :options="getOptions()" :value-field="fields.value" :text-field="fields.text" class="mb-12 h-100" :select-size="4">
            </b-form-select>    
        </b-col>
        <b-col md="2" class="pt-4 text-center">
             <b-button class="mb-1" @click.stop=add($event.target)><i class="icon-arrow-right icons mx-2"></i></b-button>             
             <b-button @click.stop=remove($event.target)><i class="icon-arrow-left icons mx-2"></i></b-button>
        </b-col>
        <b-col md="5">            
            <div>Selected</div>          
            <b-form-select v-model="toRemove" :options="selected" :value-field="fields.value" :text-field="fields.text" class="mb-12 h-100" :select-size="4">
            </b-form-select>        
        </b-col>
    </b-row>    
</template>
<script>
export default {
  name: "v-selector",
  props: ["model", "modelOptions", "valueKey", "textKey"],
  data() {
    return {
      toAdd: null,
      toRemove: null,
      selected: this.model,
      options: this.modelOptions,
      fields: {
        value: this.valueKey,
        text: this.textKey
      }
    };
  },
  methods: {
    getOptions() {
      return this.options.filter(
        x =>
          !this.selected.find(
            s => s[this.fields.value] === x[this.fields.value]
          )
      );
    },
    add(e) {
      let addObj = this.options.find(x => x[this.fields.value] === this.toAdd);
      if (addObj) {
        this.selected.push(addObj);
      }
    },
    remove(e) {
      let removeObj = this.selected.find(
        x => x[this.fields.value] === this.toRemove
      );
      if (removeObj) {
        this.selected.splice(this.selected.indexOf(removeObj), 1);
      }
    },
    refresh(items) {
      this.selected = items;
    }
  }
};
</script>
