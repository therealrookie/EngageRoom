const localuseModule = (() => {
  "use strict";

  function onInit() {
    const localUsePage = document.getElementById("localuse-page");
    const ledControl = localUsePage.querySelector("#ledControlButton");
    const monitorControl = localUsePage.querySelector("#monitorControlButton");
    const cameraControl = localUsePage.querySelector("#cameraControlButton");
    const homeButton = localUsePage.querySelector("#homeButtonColumn");
    const backButton = localUsePage.querySelector("#backButtonColumn");

    // --------- SEND VOLUME HAS CHANGED EVENTS -----------------------------------------

    CrComLib.subscribeState("n", "localUse.brandMusicVolumeFb", (value) => {
      if (value >= 0 && value <= 65535) {
        CrComLib.publishEvent("b", "localUse.volumeChangedBrandMusic", true);
        CrComLib.publishEvent("b", "localUse.volumeChangedBrandMusic", false);
      }
    });

    CrComLib.subscribeState("n", "localUse.micVolumeFb", (value) => {
      if (value >= 0 && value <= 65535) {
        CrComLib.publishEvent("b", "localUse.volumeChangedMicVolume", true);
        CrComLib.publishEvent("b", "localUse.volumeChangedMicVolume", false);
      }
    });

    CrComLib.subscribeState("n", "localUse.mediaLevelFb", (value) => {
      if (value >= 0 && value <= 65535) {
        CrComLib.publishEvent("b", "localUse.volumeChangedMediaLevel", true);
        CrComLib.publishEvent("b", "localUse.volumeChangedMediaLevel", false);
      }
    });

    ledControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 4);
    });

    monitorControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 5);
    });

    cameraControl.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 6);
    });

    homeButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 1);
    });

    backButton.addEventListener("click", () => {
      CrComLib.publishEvent("n", "controlPages.page", 2);
    });
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:localuse-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:localuse-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
