<template>
    <div class="animated fadeIn">
        <b-row v-if="model">
            <b-col md="6">
                <b-card class="text-white bg-primary">
                    <div class="h1 text-muted text-right mb-4">
                        <i class="icon-briefcase"></i>
                    </div>
                    <div class="h4 mb-0">{{model.totalOffers}}</div>
                    <small class="text-muted text-uppercase font-weight-bold">Offers</small>
                    <b-progress height={} class="progress-white progress-xs mt-3" :value="getOffersPercentage()"/>
                </b-card>
            </b-col>            
            <b-col md="6">
                <b-card class="text-white bg-info">
                    <div class="h1 text-muted text-right mb-4">
                        <i class="icon-user"></i>
                    </div>
                    <div class="h4 mb-0">{{model.totalEmployees}}</div>
                    <small class="text-muted text-uppercase font-weight-bold">Employees</small>
                    <b-progress height={} class="progress-white progress-xs mt-3" :value="getEmployeesPercentage()"/>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>
<script>
export default {
    name:"Dashboard",
    data(){
        return {
            model:null
        }
    },
    methods:{
       getOffersPercentage(){
           let total = this.model.totalEmployees+this.model.totalOffers
           return Math.round((this.model.totalOffers/total)*100)
       },
       getEmployeesPercentage(){
           let total = this.model.totalEmployees+this.model.totalOffers
           return Math.round((this.model.totalEmployees/total)*100)
       } 
    }, 
    mounted() {
        let self = this;
        axios
            .get("http://es-workspace-mikhails85.c9users.io:8081/api/statistic/totaldocs")
            .then(r => {
                if (r.data.success) {
                    self.model=r.data.value;
                }
            });
    }
}
</script>
