<template>
    <div class="animated fadeIn">
        <div class="card" v-if="offer">
            <div class="card-header">
                #{{offer.id}}:{{offer.name}}
            </div>
            <div class="card-body">
                <b-row>
                    <b-col md="6">
                        <v-form :frm-model="offer" :frm-fields="formFields"></v-form>
                    </b-col>
                    <b-col md="6">
                        <div class="mb-1">Skills:</div>
                        <v-selector :model="offer.requaredSkills" :model-options="skills" value-key="id" text-key="name"></v-selector>
                    </b-col>
                </b-row>
                <b-row>
                    <b-col md="12">
                        <b-button size="lg" variant="primary" @click.stop="updateOffer" class="mr-1">Save changes</b-button>
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
export default {
  name: "OfferDetails",
  components: {
    "v-selector": selector,
    "v-form": form
  },
  data() {
    return {
      id: this.$route.params.id,
      offer: null,
      skills: null,
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
      ]
    };
  },
  methods: {
    updateOffer() {
      axios.put(
        "http://es-workspace-mikhails85.c9users.io:8081/api/offers/" +
          this.id +
          "/updateoffer",
        this.offer
      );
    }
  },
  mounted() {
    axios
      .get("http://es-workspace-mikhails85.c9users.io:8081/api/skills/list")
      .then(sr => {
        if (!sr.data.success) {
          return;
        }

        this.skills = sr.data.value;
        axios
          .get(
            "http://es-workspace-mikhails85.c9users.io:8081/api/offers/" +
              this.id +
              "/getoffer"
          )
          .then(r => {
            if (!r.data.success) {
              return;
            }
            this.offer = r.data.value;
            if (!this.offer.requaredSkills) {
              this.offer.requaredSkills = [];
            }
          });
      });
  }
};
</script>
