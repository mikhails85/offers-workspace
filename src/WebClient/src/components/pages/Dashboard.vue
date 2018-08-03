<template>
    <div class="animated fadeIn">
        <b-row v-if="model">
            <b-col md="6">
                <b-card class="text-white bg-primary">
                    <div class="h1 text-muted text-right mb-4">
                        Offers <i class="icon-briefcase"></i>
                    </div>
                    <div class="h4 mb-0">{{model.totalOffers}}</div>
                    <small class="text-muted text-uppercase font-weight-bold">Offers / Employees</small>
                    <b-progress height={} class="progress-white progress-xs mt-3" :value="getOffersPercentage()"/>
                </b-card>
            </b-col>            
            <b-col md="6">
                <b-card class="text-white bg-info">
                    <div class="h1 text-muted text-right mb-4">
                       Employees <i class="icon-user"></i>
                    </div>
                    <div class="h4 mb-0">{{model.totalEmployees}}</div>
                    <small class="text-muted text-uppercase font-weight-bold">Employees / Offers</small>
                    <b-progress height={} class="progress-white progress-xs mt-3" :value="getEmployeesPercentage()"/>
                </b-card>
            </b-col>
        </b-row>
        <b-row>
            <b-col md="6" v-if="oskills">
                <b-card>
                    <v-chart :model-labels="oskills.keys" :model="oskills.values"></v-chart>
                </b-card>
            </b-col>
            <b-col md="6" v-if="eskills">
                <b-card>
                    <v-chart :model-labels="eskills.keys" :model="eskills.values" ></v-chart>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>
<script>
import axios from "axios";
import chart from "../controls/Chart.vue";

export default {
  name: "Dashboard",
  components: {    
    "v-chart": chart
  },
  data() {
    return {
      model: null,
      oskills: null,
      eskills: null  
    };
  },
  methods: {
    getOffersPercentage() {
      let total = this.model.totalEmployees + this.model.totalOffers;
      return Math.round((this.model.totalOffers / total) * 100);
    },
    getEmployeesPercentage() {
      let total = this.model.totalEmployees + this.model.totalOffers;
      return Math.round((this.model.totalEmployees / total) * 100);
    }
  },
  mounted() {
    let self = this;
    axios
      .get(
        "http://es-workspace-mikhails85.c9users.io:8081/api/statistic/totaldocs"
      )
      .then(r => {
        if (r.data.success) {
          self.model = r.data.value;
        }
      });
    axios
      .get(
        "http://es-workspace-mikhails85.c9users.io:8081/api/statistic/offersskills"
      )
      .then(r => {
        if (r.data.success) {
          self.oskills = {keys:[], values:[]}  
          for (var k in r.data.value) {                
            self.oskills.keys.push(k); 
            self.oskills.values.push(r.data.value[k]);     
          }          
        }
      });
    axios
      .get(
        "http://es-workspace-mikhails85.c9users.io:8081/api/statistic/employeesskills"
      )
      .then(r => {
        if (r.data.success) {
          self.eskills = {keys:[], values:[]}  
          for (var k in r.data.value) {                
            self.eskills.keys.push(k); 
            self.eskills.values.push(r.data.value[k]);     
          }    
        }
      });
  }
};
</script>
