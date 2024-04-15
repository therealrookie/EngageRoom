const selectlocalityModule = (() => {
  "use strict";

  function onInit() {
    const page1 = document.getElementById("selectlocality-page");
    const localUseButton = page1.querySelector("#" + "localUseColumn");
    const meetingButton = page1.querySelector("#" + "meetingColumn");
    const homeButton = page1.querySelector("#" + "navButton");

    localUseButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 3);
    });

    meetingButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 7);
      meetingPageControl();
    });

    homeButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 1);
    });

    function meetingPageControl() {
      CrComLib.publishEvent("b", "controlPages.meetingActivated", true);
      CrComLib.publishEvent("b", "controlPages.meetingActivated", false);
    }
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:selectlocality-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:selectlocality-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
