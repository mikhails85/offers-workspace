<template>
    <div class="animated fadeIn">
        <div class="card">
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
// import axios from "axios";
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
      alert(JSON.stringify(this.offer));
    }
  },
  mounted() {
    this.offer = {
      id: this.id,
      name: "TP",
      description: "Test Project",
      requaredSkills: []
    };
    this.skills = [
      { id: 1, name: "Skill 1" },
      { id: 2, name: "Skill 2" },
      { id: 3, name: "Skill 3" }
    ];
  }
};
</script>
