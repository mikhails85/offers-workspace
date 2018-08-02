<template>
    <div class="animated fadeIn">
        <div class="card" v-if="employee">
            <div class="card-header">
                #{{employee.id}}:{{employee.name}}
            </div>
            <div class="card-body">
                <b-row>
                    <b-col md="6">
                        <v-form :frm-model="employee" :frm-fields="formFields"></v-form>
                        <b-button size="lg" variant="primary" @click.stop="updateEmployee" class="mr-1">Save changes</b-button>
                    </b-col>
                    <b-col md="6">
                        <div>
                          Projects:
                        </div>
                        <v-table ref="table" :tbl-data="employee.projects" :tbl-fields="projFields" :on-create="createProj" :on-delete="removeProj"></v-table>
                        <b-modal id="modalAdd" @hide="resetAddProjModal" @ok="addProject" title="New Project">
                            <v-form :frm-model="newProject" :frm-fields="formProjFields"></v-form>
                            <div class="mb-1">Skills:</div>
                            <div class="mb-1"><v-selector ref="selector" :model="newProject.usedSkills" :model-options="skills" value-key="id" text-key="name"></v-selector></div>
                        </b-modal>                
                    </b-col>  
                </b-row>
            </div>
        </div>
    </div>
</template>
<script>
import axios from "axios";
import selector from "../controls/Selector.vue";
import form from "../controls/Form.vue";
import table from "../controls/Table.vue";

export default {
  name: "EmployeeDetails",
  components: {
    "v-table": table,
    "v-selector": selector,
    "v-form": form
  },
  data() {
    return {
      id: this.$route.params.id,
      skills: null,
      timer: null,
      employee: null,
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
      newProject: { name: "", description: "", usedSkills: [] },
      formProjFields: [
        {
          key: "name",
          id: "OfferName",
          type: "text",
          label: "Offer",
          placeholder: "Enter offer name"
        },
        {
          key: "description",
          id: "DescriptionName",
          type: "lng-text",
          label: "Description",
          placeholder: "Enter offer description"
        }
      ],
      projFields: [
        { key: "id", label: "ID", sortable: true },
        { key: "name", label: "Offer", sortable: true },
        { key: "actions", label: "Actions", class: "sm" }
      ]
    };
  },
  methods: {
    updateEmployee() {
      axios.put(
        "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
          this.id +
          "/updateemployee",
        this.employee
      );
    },
    resetAddProjModal() {
      this.newProject.name = "";
      this.newProject.description = "";
      this.newProject.usedSkills = [];
      this.$refs.selector.refresh(this.newProject.usedSkills);
    },
    addProject(evt) {
      evt.preventDefault();
      axios
        .post(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
            this.id +
            "/addproject",
          {
            EmployeeId: this.id,
            Name: this.newProject.name,
            Description: this.newProject.description,
            UsedSkills: this.newProject.usedSkills
          }
        )
        .then(r => {
          this.refreshProjects();
        });

      this.$root.$emit("bv::hide::modal", "modalAdd", null);
    },
    createProj() {
      this.$root.$emit("bv::show::modal", "modalAdd", null);
    },
    removeProj(item) {
      axios
        .delete(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
            this.id +
            "/deleteproject/" +
            item.id
        )
        .then(r => {
          this.refreshProjects();
        });
    },
    refreshProjects() {
      let self = this;
      axios
        .get(
          "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
            this.id +
            "/getprojects"
        )
        .then(r => {
          if (!r.data.success) {
            return;
          }
          self.employee.projects = r.data.value;
          self.$refs.table.refresh(self.employee.projects);
        });
    }
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
  mounted() {
    this.timer = setInterval(this.refreshProjects, 10000);
    axios
      .get("http://es-workspace-mikhails85.c9users.io:8081/api/skills/list")
      .then(sr => {
        if (!sr.data.success) {
          return;
        }

        this.skills = sr.data.value;
        axios
          .get(
            "http://es-workspace-mikhails85.c9users.io:8081/api/employees/" +
              this.id +
              "/getemployee"
          )
          .then(r => {
            if (!r.data.success) {
              return;
            }
            this.employee = r.data.value;
            if (!this.employee.projects) {
              this.employee.projects = [];
            }
          });
      });
  }
};
</script>
