<template>
  <div class="animated fadeIn">
    <div class="card">
      <div class="card-header">
        Offers
      </div>
      <div class="card-body">
        <v-table ref="table" :tbl-data="items" :tbl-fields="fields" :on-next-page="getNextPage" :on-create="create" :on-update="update" :on-delete="remove" :on-search="search"></v-table>
        <b-modal id="modalAdd" @hide="resetAddModal" @ok="addItem" title="New item">
            <v-form :frm-model="newOffer" :frm-fields="formFields"></v-form>
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
  name: "Offers",
  components: {
    "v-table": table,
    "v-form": form
  },
  data() {
    return {
      timer: null,
      newOffer: { name: "", description: "" },
      formFields: [
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
      items: [],
      fields: [
        { key: "id", label: "ID", sortable: true },
        { key: "name", label: "Offer", sortable: true },
        { key: "requaredSkills", label: "Skills" },
        { key: "actions", label: "Actions", class: "sm" }
      ]
    };
  },
  methods: {
    resetAddModal() {
      this.newOffer.name = "";
      this.newOffer.description = "";
    },
    addItem(evt) {
      evt.preventDefault();
      axios
        .post(
          "http://es-workspace-mikhails85.c9users.io:8081/api/offers/addoffer",
          {
            Name: this.newOffer.name,
            Description: this.newOffer.description
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
      this.$router.push({ name: "Offer", params: { id: item.id } });
    },
    remove(item) {
      axios
        .delete(
          "http://es-workspace-mikhails85.c9users.io:8081/api/offers/" +
            item.id +
            "/deleteoffer"
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
          "http://es-workspace-mikhails85.c9users.io:8081/api/offers/list?page=0&size=" +
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
