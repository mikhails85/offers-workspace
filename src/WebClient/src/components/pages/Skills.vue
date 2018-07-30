<template>
  <div class="animated fadeIn">
    <div class="card">
      <div class="card-header">
        Skills
      </div>
      <div class="card-body">
        <v-table ref="table" :tbl-data="items" :tbl-fields="fields" :on-create="create" ></v-table>
        <b-modal id="modalAdd" @hide="resetAddModal" @ok="addItem" title="New item">
            <v-form :frm-model="newSkill" :frm-fields="formFields"></v-form>
        </b-modal>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import table from "../controls/Table.vue";
import form from "../controls/Form.vue";
export default {
  name: "Skills",
  components: {
    "v-table": table,
    "v-form": form
  },
  data() {
    return {
      items: [],
      fields: [
        { key: "id", label: "ID", sortable: true },
        { key: "name", label: "Name", sortable: true }
      ],
      formFields: [
        {
          key: "name",
          id: "SkillName",
          type: "text",
          label: "Skill",
          placeholder: "Enter skill"
        }
      ],
      newSkill: {
        name: ""
      }
    };
  },
  methods: {
    resetAddModal() {
      this.newSkill.name = "";
    },
    addItem(evt) {
      evt.preventDefault();
      axios.post("/api/skills/addskill", {
        Name: this.newSkill.name
      });
      this.refresh();
      this.$root.$emit("bv::hide::modal", "modalAdd", null);
    },
    create() {
      this.$root.$emit("bv::show::modal", "modalAdd", null);
    },
    refresh() {
      let self = this;
      axios.get("/api/skills/list").then(r => {
        if (r.data.success) {
          self.$refs.table.refresh(r.data.value);
        }
      });
    }
  },
  mounted() {
    this.refresh();
  }
};
</script>
