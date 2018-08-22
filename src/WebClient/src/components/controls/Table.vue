<template>
    <div>
    <b-row class="my-2">
        <b-col md="6">
            <b-form-group v-if="onSearch" horizontal label="Filter" class="mb-0">
                <b-input-group>
                    <b-form-input v-model="search" placeholder="Type to Search" />
                    <b-input-group-append>
                    <b-btn :disabled="!search" @click="search = ''">Clear</b-btn>
                    </b-input-group-append>
                </b-input-group>
            </b-form-group>
        </b-col>
        <b-col md="6" class="text-right">
            <b-button @click.stop="openAdd($event.target)" variant="success" size="md"><i class="icon-plus icons mx-2"></i>Add</b-button>
        </b-col>
    </b-row>
    <b-table ref="table" show-empty
            stacked="md"
            :items="items"
            :fields="fields"
    >
        <template slot="usedSkills" slot-scope="row">
            <b-badge v-for="skill in row.item.usedSkills" class="mr-1" :key="skill.id" >{{skill.name}}</b-badge>
        </template>  
        <template slot="requaredSkills" slot-scope="row">
            <b-badge v-for="skill in row.item.requaredSkills" class="mr-1" :key="skill.id" >{{skill.name}}</b-badge>
        </template>  
        <template slot="actions" slot-scope="row">
            <!-- We use @click.stop here to prevent a 'row-clicked' event from also happening -->
            <b-button v-if="onUpdate" size="sm" variant="warning" @click.stop="openEdit(row.item, row.index, $event.target)" class="mr-1">
                <i class="icon-pencil  icons mx-2"></i>
            </b-button>
            <b-button size="sm" variant="danger" @click.stop="removeRow(row.item, row.index, $event.target)" class="mr-1">
                <i class="icon-trash icons mx-2"></i>
            </b-button>
        </template>
    </b-table>
    <b-row v-if="onNextPage">
        <b-col md="12" class="text-center">
            <b-button size="lg" variant="primary" @click.stop="moreItems" class="mr-1">
                More Items
                <i class="icon-arrow-down d-block mt-2 icons mx-2"></i>
            </b-button>
        </b-col>
    </b-row>
    </div>
</template>
<script>
export default {
  name: "v-table",
  data() {
    return {
      items: this.tblData,
      fields: this.tblFields,
      search: null
    };
  },
  watch: {
    search(after, before) {
      this.onSearch(this.search);
    }
  },
  props: [
    "tblData",
    "tblFields",
    "onNextPage",
    "onCreate",
    "onUpdate",
    "onDelete",
    "onSearch"
  ],
  methods: {
    openAdd(button) {
      this.onCreate();
    },
    openEdit(item, index, button) {
      this.onUpdate(item);
    },
    removeRow(item, index, button) {
      this.onDelete(item);
    },
    moreItems() {
      this.onNextPage();
    },
    refresh(items) {
      this.items = items;
    }
  }
};
</script>
<style>
td.sm,
th.sm {
  width: 10%;
}
</style>
