<template>
  <div class="animated fadeIn">
    <div class="card">
      <div class="card-header">
        Employees
      </div>
      <div class="card-body">
        <v-table ref="table" :tbl-data="items" :tbl-fields="fields" :on-next-page="getNextPage" :on-create="create" :on-update="update" :on-delete="remove" :on-search="search"></v-table>
        <b-modal id="modalAdd" @hide="resetAddModal" @ok="addItem" title="New item">
            <v-form :frm-model="newEmployee" :frm-fields="formFields"></v-form>
        </b-modal>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import table from "../controls/Table.vue";
import form from "../controls/Form.vue";

let page = 0;
let size = 50;
let searching = "";

export default {
  name: "Employees",
  components: {
    "v-table": table,
    "v-form": form
  },
  data() {
    return {
      timer: null,
      newEmployee: { name: "", jobTitle: "", photo: "", cv: "" },
      formFields: [
        {
          key: "name",
          id: "EmployeeName",
          type: "text",
          label: "Full Name",
          placeholder: "Enter employee full name"
        },
        {
          key: "jobTitle",
          id: "EmployeeJobTitle",
          type: "text",
          label: "Job Title",
          placeholder: "Enter employee job title"
        },
        {
          key: "photo",
          id: "EmployeePhoto",
          type: "text",
          label: "Photo",
          placeholder: "Enter employee photo url"
        },
        {
          key: "cv",
          id: "EmployeeCV",
          type: "text",
          label: "CV",
          placeholder: "Enter employee cv url"
        }
      ],
      items: [],
      fields: [
        { key: "id", label: "ID", sortable: true },
        { key: "name", label: "Full Name", sortable: true },
        { key: "jobTitle", label: "Job Title", sortable: true },
        { key: "actions", label: "Actions", class: "sm" }
      ]
    };
  },
  methods: {
    resetAddModal() {
      this.newEmployee.name = "";
      this.newEmployee.jobTitle = "";
      this.newEmployee.photo = "";
      this.newEmployee.cv = "";
    },
    addItem(evt) {
      evt.preventDefault();
      axios
        .post(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/addemployee",
          {
            Name: this.newEmployee.name,
            JobTitle: this.newEmployee.jobTitle,
            Photo: this.newEmployee.photo,
            CV: this.newEmployee.cv
          }
        )
        .then(r => {
          this.refresh();
        });

      this.$root.$emit("bv::hide::modal", "modalAdd", null);
    },
    create() {
      this.$root.$emit("bv::show::modal", "modalAdd", null);
    },
    update(item) {
      this.$router.push({ name: "Employee", params: { id: item.id } });
    },
    remove(item) {
      axios
        .delete(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
            item.id +
            "/deleteemployee"
        )
        .then(r => {
          this.refresh();
        });
    },
    search(text) {
      searching = text;
      page = 0;
      this.refresh();
    },
    getNextPage() {
      page = page + 1;
      this.refresh();
    },
    refresh() {
      let self = this;
      axios
        .get(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/list?page=0&size=" +
            (page + 1) * size +
            "&search=" +
            searching
        )
        .then(r => {
          if (r.data.success) {
            self.$refs.table.refresh(r.data.value);
          }
        });
    }
  },
  mounted() {
    this.refresh();
    this.timer = setInterval(this.refresh, 10000);
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>
