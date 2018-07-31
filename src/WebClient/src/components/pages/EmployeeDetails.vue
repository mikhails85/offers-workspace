<template>
    <b-row>
        <b-col>
            //TODO: {{Id}}
        </b-col>
    </b-row>
</template>
<script>
export default {
  name: "EmployeeDetails",
  data() {
    return { Id: this.$route.params.id };
  }
};
</script>
