const cameracontrolModule = (() => {
  "use strict";

  function onInit() {
    // -------------------------------- CONSTANTS --------------------------------

    const cameraControlPage = document.getElementById("cameracontrol-page");
    const homeButton = cameraControlPage.querySelector("#homeButtonColumn");
    const backButton = cameraControlPage.querySelector("#backButtonColumn");
    const zoomPlusButton = cameraControlPage.querySelector("#zoomPlus");
    const zoomMinusButton = cameraControlPage.querySelector("#zoomMinus");

    // -------------------------------- VARIABLES --------------------------------

    // ----------------------------- PRESET BUTTONS --------------------------------------------------------------------------------------

    let presetButtons = [
      {
        event: "cameraControl.preset01",
        feedback: "cameraControl.preset01Fb",
        value: false,
        id: "preset01Button",
      },
      {
        event: "cameraControl.preset02",
        feedback: "cameraControl.preset02Fb",
        value: false,
        id: "preset02Button",
      },
      {
        event: "cameraControl.preset03",
        feedback: "cameraControl.preset03Fb",
        value: false,
        id: "preset03Button",
      },
      {
        event: "cameraControl.call",
        feedback: "cameraControl.callFb",
        value: false,
        id: "callButton",
      },
      {
        event: "cameraControl.setBtn",
        feedback: "cameraControl.setBtnFb",
        value: false,
        id: "setButton",
      },
    ];

    // GET PRESET BUTTONS FEEDBACK
    presetButtons.forEach((button) => {
      CrComLib.subscribeState("b", button.feedback, (value) => {
        button.value = value;
        updateActivatedStyle(button);
      });
    });

    // LISTEN ON ALL PRESET BUTTONS
    presetButtons.forEach((button) => {
      const btnElement = cameraControlPage.querySelector(`#${button.id}`);
      if (button.id === "setButton") {
        btnElement.addEventListener("touchstart", () => {
          CrComLib.publishEvent("b", "cameraControl.setBtn", true);
        });
        btnElement.addEventListener("touchend", () => {
          CrComLib.publishEvent("b", "cameraControl.setBtn", false);
          setPresetButtonsToZero();
        });
        btnElement.addEventListener("touchcancel", () => {
          CrComLib.publishEvent("b", "cameraControl.setBtn", false);
          setPresetButtonsToZero();
        });
      } else {
        btnElement.addEventListener("click", () => {
          sendPressedPresetButton(button.id);
        });
      }
    });

    // SEND CORRECT VALUES FOR EACH BUTTON
    function sendPressedPresetButton(buttonId) {
      presetButtons.forEach((button, index) => {
        // if button = preset button
        if (index < 3) {
          // if button is pressed
          if (button.id === buttonId) {
            CrComLib.publishEvent("b", button.event, true);
            CrComLib.publishEvent("b", button.event, false);
            CrComLib.publishEvent("b", button.event, true);
            // all other buttons, that are not pressed
          } else {
            CrComLib.publishEvent("b", button.event, false);
          }
          // if button = preview button
        } else if (index == 3) {
          // if button is pressed
          if (button.id === buttonId) {
            CrComLib.publishEvent("b", button.event, !button.value);
          }
        }
      });
    }

    function setPresetButtonsToZero() {
      presetButtons.forEach((button, index) => {
        index < 3 && CrComLib.publishEvent("b", button.event, false);
      });
    }

    // UPDATE STYLE OF PRESET BUTONS
    function updateActivatedStyle(button) {
      const element = cameraControlPage.querySelector("#" + button.id);
      if (button.id === "setButton") {
        button.value ? element.classList.add("setButtonPressed") : element.classList.remove("setButtonPressed");
      } else {
        button.value ? element.classList.add("buttonPressed") : element.classList.remove("buttonPressed");
      }
    }

    // ----------------------------- ZOOM CONTROL --------------------------------------------------------------------------------------

    let zoomTimeout;
    let isZoomingIn = false;
    let isZoomingOut = false;

    // ZOOM PLUS
    CrComLib.subscribeState("b", "cameraControl.zoomPlusFb", (zoomPlus) => {
      if (zoomPlus) {
        zoomPlusButton.classList.add("buttonPressed");
      } else {
        zoomPlusButton.classList.remove("buttonPressed");
      }
    });

    // ZOOM MINUS
    CrComLib.subscribeState("b", "cameraControl.zoomMinusFb", (zoomMinus) => {
      if (zoomMinus) {
        zoomMinusButton.classList.add("buttonPressed");
      } else {
        zoomMinusButton.classList.remove("buttonPressed");
      }
    });

    function zoomIn() {
      CrComLib.publishEvent("b", "cameraControl.zoomPlus", true);
    }

    // Function to handle zoom out
    function zoomOut() {
      CrComLib.publishEvent("b", "cameraControl.zoomMinus", true);
    }

    zoomPlusButton.addEventListener("touchstart", (event) => {
      event.preventDefault(); // Prevent default touch behavior (e.g., scrolling)
      if (!isZoomingOut) {
        isZoomingIn = true;
        zoomIn(); // Trigger zoomIn immediately
        // Start repeating action when the button is touched and held
        zoomTimeout = setInterval(zoomIn, 200); // Adjust the interval as needed
      }
    });

    // Add touchend event listener to stop zooming in
    zoomPlusButton.addEventListener("touchend", () => {
      isZoomingIn = false;
      CrComLib.publishEvent("b", "cameraControl.zoomPlus", false);
      clearInterval(zoomTimeout);
    });

    // Add touchstart event listener for zooming out
    zoomMinusButton.addEventListener("touchstart", (event) => {
      event.preventDefault(); // Prevent default touch behavior (e.g., scrolling)
      if (!isZoomingIn) {
        isZoomingOut = true;
        zoomOut(); // Trigger zoomOut immediately
        // Start repeating action when the button is touched and held
        zoomTimeout = setInterval(zoomOut, 200); // Adjust the interval as needed
      }
    });

    // Add touchend event listener to stop zooming out
    zoomMinusButton.addEventListener("touchend", () => {
      isZoomingOut = false;
      CrComLib.publishEvent("b", "cameraControl.zoomMinus", false);
      clearInterval(zoomTimeout);
    });

    // ----------------------------- NAVIGATION --------------------------------------------------------------------------------------

    // LISTEN ON HOME BUTTON
    homeButton.addEventListener("click", function () {
      CrComLib.publishEvent("n", "controlPages.page", 1);
    });

    // LISTEN ON BACK BUTTON
    backButton.addEventListener("click", function () {
      CrComLib.publishEvent("n", "controlPages.page", 3);
    });
  }

  let loadedSubId = CrComLib.subscribeState("o", "ch5-import-htmlsnippet:cameracontrol-import-page", (value) => {
    if (value["loaded"]) {
      onInit();
      setTimeout(() => {
        CrComLib.unsubscribeState("o", "ch5-import-htmlsnippet:cameracontrol-import-page", loadedSubId);
        loadedSubId = "";
      });
    }
  });

  return {};
})();
