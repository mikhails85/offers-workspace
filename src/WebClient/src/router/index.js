import Vue from "vue";
import Router from "vue-router";

import Dashboard from "../components/pages/Dashboard";
import Skills from "../components/pages/Skills";
import Offers from "../components/pages/Offers";
import Employees from "../components/pages/Employees";
import OfferDetails from "../components/pages/OfferDetails";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "Dashboard",
      component: Dashboard
    },
    {
      path: "/skills",
      name: "Skills",
      component: Skills
    },
    {
      path: "/offers",
      name: "Offers",
      component: Offers
    },
    {
      path: "/offers/:id",
      name: "Offers",
      component: { template: "<router-view></router-view>" },
      children: [
        {
          path: "",
          name: "Offer",
          component: OfferDetails
        }
      ]
    },
    {
      path: "/employees",
      name: "Employees",
      component: Employees
    }
  ],
  mode: "history"
});
