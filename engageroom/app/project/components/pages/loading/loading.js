const loadingModule = (() => {
  "use strict";

  function onInit() {
    const loadingText = document.getElementById("loading-text");
    let isEnglish = true;
    let dotCount = 0;

    function updateText() {
      let text = isEnglish ? "System is loading" : "Le système est en cours de chargement";
      for (let i = 0; i < dotCount; i++) {
        text += " .";
      }
      loadingText.textContent = text;
    }

    function animate() {
      updateText();
      dotCount++;

      if (dotCount > 3) {
        dotCount = 0;
        isEnglish = !isEnglish; // Toggle language after full dot animation
      }

      setTimeout(animate, 1000); // Call animate every 500ms
    }

    animate(); // Start the animation
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:loading-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:loading-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
