<template>
    <div>
        <b-form-group v-for="field in fields" :key="field.id" :id="field.id+'InputGroup'"
                        :label="field.label+':'"
                        :label-for="field.id+'Input'"
                        :description="field.description">
            <b-form-input v-if="isInputField(field.type)" :id="field.id+'Input'"
                        :type="field.type"
                        v-model="model[field.key]"
                        required
                        :placeholder="field.placeholder">
            </b-form-input>
            <b-form-checkbox v-if="field.type=='bool'" :id="field.id+'Input'" v-model="model[field.key]" :indeterminate.sync="indeterminate">
                {{field.placeholder}}
            </b-form-checkbox>
            <b-form-select v-if="field.type=='select'" :id="field.id+'Input'"
                      :options="field.options"
                      v-model="model[field.key]"/>
            <b-form-textarea v-if="field.type=='lng-text'" :id="field.id+'Input'"
                     v-model="model[field.key]"
                     :placeholder="field.placeholder"
                     :rows="3"
                     :max-rows="6">
            </b-form-textarea>
        </b-form-group>
    </div>
</template>
<script>
export default {
  name: "v-form",
  data() {
    return {
      fields: this.frmFields,
      model: this.frmModel
    };
  },
  props: ["frmFields", "frmModel"],
  methods: {
    isInputField(type) {
      return (
        type === "text" ||
        type === "password" ||
        type === "email" ||
        type === "number" ||
        type === "date" ||
        type === "time"
      );
    }
  }
};
</script>
