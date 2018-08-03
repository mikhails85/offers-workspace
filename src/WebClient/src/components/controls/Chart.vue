<script>
import { Doughnut } from "vue-chartjs";
export default {
  extends: Doughnut,
  name: "v-chart",
  props: ["modelLabels", "model"],
  mounted() {
    this.renderChart(
      {
        labels: this.modelLabels,
        datasets: [
          {
            backgroundColor: ["#41B883", "#E46651", "#00D8FF", "#DD1B16"],
            data: this.model
          }
        ]
      },
      { responsive: true, maintainAspectRatio: true }
    );
  }
};
</script>
