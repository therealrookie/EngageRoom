
/* global CrComLib, projectConfigModule, loggerService, templateVersionInfoModule */

const featureModule = (() => {
  'use strict';

  let themeTimer = null;

  /**
   * This is public method to change the theme
   * @param {string} theme pass theme type like 'light-theme', 'dark-theme'
   */
  function changeTheme(theme) {
    clearTimeout(themeTimer);
    themeTimer = setTimeout(() => {
      projectConfigModule.projectConfigData().then((response) => {
        let selectedTheme;
        let body = document.body;
        for (let i = 0; i < response.themes.length; i++) {
          body.classList.remove(response.themes[i].name);
        }
        let selectedThemeName = "";
        if (theme && theme !== "") {
          selectedThemeName = theme.trim();
        } else {
          selectedThemeName = response.selectedTheme.trim();
        }
        body.classList.add(selectedThemeName);
        selectedTheme = response.themes.find((tempObj) => tempObj.name.trim().toLowerCase() === selectedThemeName.trim().toLowerCase());

        if (document.getElementById("brandLogo")) {
          document.getElementById("brandLogo").setAttribute("url", selectedTheme.brandLogo.url);
        }
      });
    }, 500);
  }

  /**
   * Initialize remote logger
   * @param {string} hostName - docker server IPaddress / Hostname
   * @param {string} portNumber - docker server Port number
   */
  function initializeLogger(hostName, portNumber) {
    setTimeout(() => {
      loggerService.setRemoteLoggerConfig(hostName, portNumber);
    });
  }

  /**
   * Log information in specific interval as mentioned in project-config.json
   * @param {string} duration duration to log issues
   * @returns 
   */
  function logDiagnostics(duration) {
    let delay = 0;
    let logInterval;
    if (duration === "none") {
      return;
    } else if (duration === "hourly") {
      delay = 60 * 60 * 1000; // 1 hour in msec
    } else if (duration === "daily") {
      delay = 60 * 60 * 1000 * 24; // 24 hour in msec
    } else if (duration === "weekly") {
      delay = 60 * 60 * 1000 * 24 * 7; // Weekly in msec
    }

    if (!logInterval) {
      logInterval = setInterval(templateVersionInfoModule.logSubscriptionsCount, delay);
    }
  }

  /**
   * All public method and properties exporting here
   */
  return {
    changeTheme,
    initializeLogger,
    logDiagnostics
  };

})();

/*jslint es6 */
/*global CrComLib, projectConfigModule, navigationModule, templatePageModule, translateModule, serviceModule, utilsModule, templateAppLoaderModule, templateVersionInfoModule */

const hardButtonsModule = (() => {
	'use strict';

	let repeatDigitalInterval = null;
	const REPEAT_DIGITAL_PERIOD = 200;
	const MAX_REPEAT_DIGITALS = 30000 / REPEAT_DIGITAL_PERIOD;

	let currentDevice = "";
	let currentPage = "";
	let clickedOnPage = "";

	/* 
	1. Find all unique signal names
	2. Subscribe state for all signals
	2.1. Create logic as per subscription
	*/
	function getAllSignals(hardButtonsArray) {
		const signalNames = [];
		for (let i = 0; i < hardButtonsArray.project.signals.length; i++) {
			const projectSignal = hardButtonsArray.project.signals[i];
			const signalFound = signalNames.find(signal => signal.signalName === projectSignal.hardButtonSignal);
			if (!signalFound) {
				signalNames.push({
					signalName: projectSignal.hardButtonSignal,
					isReady: false
				});
			}
		}
		for (let j = 0; j < hardButtonsArray.project.pages.length; j++) {
			const projectPage = hardButtonsArray.project.pages[j];
			for (let i = 0; i < projectPage.signals.length; i++) {
				const projectSignal = projectPage.signals[i];
				const signalFound = signalNames.find(signal => signal.signalName === projectSignal.hardButtonSignal);
				if (!signalFound) {
					signalNames.push({
						signalName: projectSignal.hardButtonSignal,
						isReady: false
					});
				}
			}
		}
		for (let k = 0; k < hardButtonsArray.project.devices.length; k++) {
			const projectDevice = hardButtonsArray.project.devices[k];
			for (let i = 0; i < projectDevice.signals.length; i++) {
				const projectSignal = projectDevice.signals[i];
				const signalFound = signalNames.find(signal => signal.signalName === projectSignal.hardButtonSignal);
				if (!signalFound) {
					signalNames.push({
						signalName: projectSignal.hardButtonSignal,
						isReady: false
					});
				}
			}
			for (let j = 0; j < projectDevice.pages.length; j++) {
				const projectPage = projectDevice.pages[j];
				for (let i = 0; i < projectPage.signals.length; i++) {
					const projectSignal = projectPage.signals[i];
					const signalFound = signalNames.find(signal => signal.signalName === projectSignal.hardButtonSignal);
					if (!signalFound) {
						signalNames.push({
							signalName: projectSignal.hardButtonSignal,
							isReady: false
						});
					}
				}
			}
		}
		return signalNames;
	}

	function initialize(deviceNameInput) {
		currentDevice = deviceNameInput;

		return new Promise((resolve, reject) => {
			serviceModule.loadJSON("./assets/data/hard-buttons.json", (dataResponse) => {
				const hardButtonData = JSON.parse(dataResponse);
				const signalNames = getAllSignals(hardButtonData);
				log("signalNames", signalNames);
				for (let i = 0; i < signalNames.length; i++) {
					const iteratedSignal = signalNames[i];
					CrComLib.subscribeState('b', iteratedSignal.signalName, (response) => {
						log("CrComLib.subscribeState: ", iteratedSignal.signalName, response, clickedOnPage);
						if (clickedOnPage !== "" || response === true) {
							if (response === true) {
								clickedOnPage = navigationModule.selectedPage();
							}
							hardButtonClicked(hardButtonData, iteratedSignal.signalName, response);
						}
					});
				}
				resolve(true);
			}, error => {
				reject(false);
			});
		});
	}

	function log(...data) {
		console.log(...data);
		let outputString = "";
		for (let i = 0; i < data.length; i++) {
			outputString += data[i] + " ";
		}
		if (document.getElementById('txtOutput')) {
			document.getElementById('txtOutput').value += outputString + "\n";
		}
	}

	function hardButtonClicked(hardButtonsArray, signal, response) {
		/* Priority is 
			(a) Device level page (if user is on the selected page)
			(b) Device level
			(c) Project level page (if user is on the selected page)
			(d) Project level
		*/
		currentPage = navigationModule.selectedPage();

		let signalValue = "";
		let navigationPageName = "";

		for (let i = 0; i < hardButtonsArray.project.signals.length; i++) {
			const selectedSignal = hardButtonsArray.project.signals[i];
			if (selectedSignal.hardButtonSignal === signal) {
				if (selectedSignal.navigationPageName !== "") {
					navigationPageName = selectedSignal.navigationPageName;
				}
				if (selectedSignal.digitalJoin !== "") {
					signalValue = selectedSignal.digitalJoin;
				}
			}
		}
		for (let j = 0; j < hardButtonsArray.project.pages.length; j++) {
			const selectedPage = hardButtonsArray.project.pages[j];
			if (selectedPage.pageName === clickedOnPage) {
				for (let i = 0; i < selectedPage.signals.length; i++) {
					const selectedSignal = selectedPage.signals[i];
					if (selectedSignal.hardButtonSignal === signal) {
						if (selectedSignal.navigationPageName !== "") {
							navigationPageName = selectedSignal.navigationPageName;
						}
						if (selectedSignal.digitalJoin !== "") {
							signalValue = selectedSignal.digitalJoin;
						}
					}
				}
			}
		}
		for (let k = 0; k < hardButtonsArray.project.devices.length; k++) {
			const selectedDevice = hardButtonsArray.project.devices[k];
			if (selectedDevice.deviceName === currentDevice) {
				for (let i = 0; i < selectedDevice.signals.length; i++) {
					const selectedSignal = selectedDevice.signals[i];
					if (selectedSignal.hardButtonSignal === signal) {
						if (selectedSignal.navigationPageName !== "") {
							navigationPageName = selectedSignal.navigationPageName;
						}
						if (selectedSignal.digitalJoin !== "") {
							signalValue = selectedSignal.digitalJoin;
						}
					}
				}
				for (let j = 0; j < selectedDevice.pages.length; j++) {
					const selectedPage = selectedDevice.pages[j];
					if (selectedPage.pageName === clickedOnPage) {
						for (let i = 0; i < selectedPage.signals.length; i++) {
							const selectedSignal = selectedPage.signals[i];
							if (selectedSignal.hardButtonSignal === signal) {
								if (selectedSignal.navigationPageName !== "") {
									navigationPageName = selectedSignal.navigationPageName;
								}
								if (selectedSignal.digitalJoin !== "") {
									signalValue = selectedSignal.digitalJoin;
								}
							}
						}
					}
				}
			}
		}

		log("signalValue: ", signalValue);
		log("navigationPageName: ", navigationPageName);
		if (navigationPageName !== "") {
			if (response === true) {
				log("currentPage.toLowerCase().trim(): ", currentPage.toLowerCase().trim());
				log("navigationPageName.toLowerCase().trim(): ", navigationPageName.toLowerCase().trim());
				if (currentPage.toLowerCase().trim() !== navigationPageName.toLowerCase().trim()) {
					templatePageModule.navigateTriggerViewByPageName(navigationPageName);
				}
			}
		}
		if (signalValue != "") {
			if (response === true) {
				CrComLib.publishEvent('b', signalValue, response);
				if (repeatDigitalInterval !== null) {
					window.clearInterval(repeatDigitalInterval);
				}
				let numRepeatDigitals = 0;
				repeatDigitalInterval = window.setInterval(() => {
					log("Prioritized signal name: ", signalValue, ' for response ', response);
					CrComLib.publishEvent('b', signalValue, response);
					if (++numRepeatDigitals >= MAX_REPEAT_DIGITALS) {
						console.warn("Hard Button MAXIMUM Repeat digitals sent");
						window.clearInterval(repeatDigitalInterval);
						CrComLib.publishEvent('b', signalValue, !response);
						if (repeatDigitalInterval !== null) {
							window.clearInterval(repeatDigitalInterval);
						}
					}
				}, REPEAT_DIGITAL_PERIOD);
			} else {
				if (repeatDigitalInterval !== null) {
					window.clearInterval(repeatDigitalInterval);
				}
				CrComLib.publishEvent('b', signalValue, response);
			}
		}
	}

	return {
		initialize
	};

})();
/*jslint es6 */
/*global CrComLib, projectConfigModule, templatePageModule, translateModule, serviceModule, utilsModule, templateAppLoaderModule, templateVersionInfoModule */

const navigationModule = (() => {
	'use strict';

	let _pageName = "";

	function goToPage(pageName) {
		const navigationPages = projectConfigModule.getAllPages();
		const pageObject = navigationPages.find(page => page.pageName === pageName);
		templateAppLoaderModule.showLoading(pageObject);
		const routeId = pageObject.pageName + "-import-page";
		const listOfPages = projectConfigModule.getNavigationPages();
		for (let i = 0; i < listOfPages.length; i++) {
			if (routeId !== listOfPages[i].pageName + "-import-page") {
				CrComLib.publishEvent('b', listOfPages[i].pageName + "-import-page-show", false);
			}
		}

		// setTimeout required because hiding is not happening instantaneously with show. Show comes first sometimes.
		setTimeout(() => {
			if (!templateAppLoaderModule.isCachePageLoaded(routeId)) {
				if (document.getElementById(routeId)) {
					const url = pageObject.fullPath + pageObject.fileName;
					document.getElementById(routeId).setAttribute("url", url);
				}
				CrComLib.publishEvent('b', routeId + '-show', true);
			}
			// LOADING INDICATOR - Uncomment the below line along with code in template-page.js file to enable loading indicator
			// CrComLib.publishEvent('b', routeId + '-show-app-loader', false);
			templatePageModule.hideLoading(pageObject); // TODO - check - fix with mutations called in callbackforhideloading

			_pageName = pageName;
			// Allow components and pages to be transitioned
			let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:' + pageObject.pageName + '-import-page', (value) => {
				if (value['loaded']) {
					const setTimeoutDelay = pageObject.preloadPage ? 0 : CrComLib.isCrestronTouchscreen() ? 300 : 50;
					setTimeout(() => updateDiagnosticsOnPageChange(pageObject.pageName), setTimeoutDelay);
					setTimeout(() => {
						CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:' + pageObject.pageName + '-import-page', loadedSubId);
						loadedSubId = '';
					});
				}
			});
		}, 50);
	}

	function selectedPage() {
		return _pageName;
	}

	function updateDiagnosticsOnPageChange(pageName) {
		projectConfigModule.projectConfigData().then((projectConfigResponse) => {
			if (projectConfigResponse.header.display === true && projectConfigResponse.header.displayInfo === true && projectConfigResponse.header.$component === ""){
				const pageImporterElement = document.getElementById(pageName + '-import-page');
				if (!pageImporterElement) return;

				// Table Count Updation
				templateVersionInfoModule.tableCount[`${pageName}`] = CrComLib.countNumberOfCh5Components(pageImporterElement);
				templateVersionInfoModule.tableCount[`${pageName}`].domNodes = pageImporterElement.getElementsByTagName('*').length;

				// Current Page Table Row Updation
				const currentPageTableRow = document.getElementById('diagnostics-table-' + pageName);
				currentPageTableRow.childNodes[1].textContent = templateVersionInfoModule.tableCount[`${pageName}`].total;
				currentPageTableRow.childNodes[4].textContent = templateVersionInfoModule.tableCount[`${pageName}`].domNodes;

				// Diagnostic Info Count Updation
				let totalDomCount = 0;
				let totalComponentsCount = 0;
				let currentCh5ComponentsCount = 0;
				const listOfPages = projectConfigModule.getNavigationPages();
				listOfPages.forEach((page) => totalDomCount += templateVersionInfoModule.tableCount[`${page.pageName}`].domNodes || 0);
				listOfPages.forEach((page) => totalComponentsCount += templateVersionInfoModule.tableCount[`${page.pageName}`].total || 0);
				listOfPages.forEach(page => {
					const pageImporterElement = document.getElementById(page.pageName + '-import-page');
					if (pageImporterElement) currentCh5ComponentsCount += CrComLib.countNumberOfCh5Components(pageImporterElement).total;
				});
				document.getElementById('totalDom').innerHTML = templateVersionInfoModule.translateModuleHelper('totalnodes', totalDomCount);
				document.getElementById('totalComponents').innerHTML = templateVersionInfoModule.translateModuleHelper('totalcomponents', totalComponentsCount);;
				document.getElementById('currentComponents').innerHTML = templateVersionInfoModule.translateModuleHelper('currentcomp', currentCh5ComponentsCount);

				// Updating Table Count for Add Log
				templateVersionInfoModule.componentCount.totalDomCount = totalDomCount;
				templateVersionInfoModule.componentCount.totalComponentsCount = totalComponentsCount;
				templateVersionInfoModule.componentCount.currentCh5Components = currentCh5ComponentsCount;
				templateVersionInfoModule.updateSubscriptions();
			}
		});
	}

	return {
		goToPage,
		selectedPage,
		updateDiagnosticsOnPageChange
	};

})();

/* global CrComLib, serviceModule, utilsModule */

const projectConfigModule = (() => {
	'use strict';

	/**
	 * All public and local properties
	 */
	let response = null;

	/**
	 * This method is used to fetch project-config.json file
	 */
	function initialize() {
		return new Promise((resolve, reject) => {
			serviceModule.loadJSON("./assets/data/project-config.json", (dataResponse) => {
				response = JSON.parse(dataResponse);
				resolve(response);
			}, error => {
				reject(error);
			});
		});
	}

	function getAllStandAloneViewPages() {
		return response.content.pages.filter((pageObj) => {
			return (!utilsModule.isValidObject(pageObj.navigation) && pageObj.standAloneView === true);
		});
	}

	function defaultActiveViewIndex() {
		let activeView = 0; //set the default
		if (response.content.$defaultView === "undefined" && response.content.$defaultView.trim() === "") {
			return activeView;
		}

		let seqObject = projectConfigModule.getNavigationPages();
		for (let i = 0; i < seqObject.length; i++) {
			if (seqObject[i].pageName.trim().toLowerCase() === response.content.$defaultView.trim().toLowerCase()) {
				activeView = i;
				break;
			}
		}
		return activeView;
	}

	function getMenuOrientation() {
		return response.menuOrientation;
	}

	function getNonNavigationPages() {
		return response.content.pages.filter(page => page.navigation === undefined);
	}

	function getNavigationPages() {
		return response.content.pages.filter(page => page.navigation !== undefined).sort(utilsModule.dynamicSort("asc", "navigation", "sequence"));
	}

	function getAllPages() {
		return response.content.pages;
	}

	function getCustomPageUrl(pageName) {
		if (pageName && pageName !== "") {
			const page = projectConfigModule.getNonNavigationPages().find(page => page.pageName === pageName);
			return page.fullPath + page.fileName;
		} else {
			return "";
		}
	}

	function getCustomFooterUrl() {
		return getCustomPageUrl(response.footer.$component);
	}

	function getCustomHeaderUrl() {
		return getCustomPageUrl(response.header.$component);
	}

	async function projectConfigData() {
		if (response !== null) {
			return response;
		} else {
			// wait until the promise returns us a value
			let result = await initialize();
			return result;
		}
	}

	/**
	 * All public method and properties exporting here
	 */
	return {
		getAllPages,
		projectConfigData,
		getNavigationPages,
		getNonNavigationPages,
		getAllStandAloneViewPages,
		defaultActiveViewIndex,
		getCustomHeaderUrl,
		getCustomFooterUrl,
		getMenuOrientation
	};

})();

// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement 
// under which you licensed this source code.

/* global CrComLib, WebXPanel, webXPanelModule */

const serviceModule = (() => {
  'use strict';
  /**
   * All public and local(prefix '_') properties
   */
  let ch5Emulator = CrComLib.Ch5Emulator.getInstance();
  let useWebXPanel;
  let initialized = false;
  let noControlSystemEmulatorScenarios = [];

  /**
   * This is public method so that we can use in other module also
   * @param {string} url pass json file path
   * @param {object} callback method to get the json response
   */
  function loadJSON(url, callback) {
    let xhr = new XMLHttpRequest();
    xhr.overrideMimeType("application/json");
    xhr.open("GET", url, true);
    xhr.onreadystatechange = function () {
      if (xhr.readyState === 4) {
        callback(xhr.responseText);
      }
    };
    xhr.send(null);
  }

  /**
   * This is public method to init the emulator
   * @param {object} emulator pass your emulator response
   */
  function initEmulator(emulator) {
    CrComLib.Ch5Emulator.clear();
    ch5Emulator = CrComLib.Ch5Emulator.getInstance();
    ch5Emulator.loadScenario(emulator);
    ch5Emulator.run();
  }

  /**
   * Load Emulator Json
   * @param {string} url 
   */
  function newJsonLoad(url) {
    // Create new promise with the Promise() constructor;
    // This has as its argument a function
    // with two parameters, resolve and reject
    return new Promise(function (resolve, reject) {
      // Standard XHR to load an image
      let request = new XMLHttpRequest();
      request.open("GET", url);
      request.responseType = "json";
      // When the request loads, check whether it was successful
      request.onload = function () {
        if (request.status === 200 || request.response !== null) {
          // If successful, resolve the promise by passing back the request response
          resolve(request.response);
        } else {
          // If it fails, reject the promise with a error message
          reject(new Error("Json didn't load successfully; error code:" + request.statusText));
        }
      };
      request.onerror = function () {
        // Also deal with the case when the entire request fails to begin with
        // This is probably a network error, so reject the promise with an appropriate message
        reject(new Error("There was a network error."));
      };
      // Send the request
      request.send();
    });
  }

  /**
   * Adding Emulator Scenario only when not running in a Crestron Touch screen
   * @param {string} url 
   */
  function addEmulatorScenarioNoControlSystem(url) {
    noControlSystemEmulatorScenarios.push(url);
    if (initialized) {
      setTimeout(drainQueuedNoControlSystemEmulatorScenarios);
    }
  }

  /**
   * Adding Emulator Scenario
   * @param {string} url 
   */
  function addEmulatorScenario(url) {
    newJsonLoad(url).then(
      (scenario) => {
        if (scenario !== null) {
          ch5Emulator.loadScenario(scenario);
          ch5Emulator.run();
        }
      },
      (err) => {
        console.error("Could not load url as json file:" + url, err);
      }
    );
  }

  function initialize(projectConfigResponse) {
    initialized = true;
    useWebXPanel = projectConfigResponse.useWebXPanel;
    drainQueuedNoControlSystemEmulatorScenarios();
  }

  function drainQueuedNoControlSystemEmulatorScenarios() {
    // CrComLib.isCrestronTouchscreen() will return true when running on TSW and mobile
    // WebXPanel.isActive will return true when when WebXPanel library can attach to control system 
    // useWebXPanel is true when project-config.json 
    // configures to use web xpanel to connect to control system using webxpanel library

    // apply scenario only 
    // not running on TSW and either No XPanel loaded or XPanel disabled 
    if (!CrComLib.isCrestronTouchscreen()
      && ((typeof WebXPanel == 'undefined' || !WebXPanel.isActive)
        || !useWebXPanel)) {
      for (let index = 0; index < noControlSystemEmulatorScenarios.length; index++) {
        const url = noControlSystemEmulatorScenarios[index];
        addEmulatorScenario(url);
      }
      noControlSystemEmulatorScenarios = [];
    }
  }

  /**
   * All public method and properties exporting here
   */
  return {
    initialize,
    loadJSON,
    initEmulator,
    addEmulatorScenario,
    addEmulatorScenarioNoControlSystem
  };

})();

// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement 
// under which you licensed this source code.
/* global CrComLib, serviceModule, utilsModule */

const translateModule = (() => {
  'use strict';
  /**
   * All public and local properties
   */
  let langData = [];
  let crComLibTranslator = CrComLib.translationFactory.translator;
  let currentLng = document.getElementById("currentLng");
  let defaultLng = "en";
  let languageTimer;
  let setLng = "en";

  /**
   * This is public method to fetch language data(JSON).
   * @param {string} lng is language code string like en, fr etc...
   */
  function getLanguage(lng) {
    if (!langData[lng]) {
      let output = {};
      loadJSON("./app/template/assets/data/translation/", lng).then((responseTemplate) => {
        output = utilsModule.mergeJSON(output, responseTemplate);
        loadJSON("./app/project/assets/data/translation/", lng).then((responseProject) => {
          output = utilsModule.mergeJSON(output, responseProject);
          langData[lng] = {
            translation: output,
          };
          setLanguage(lng);
        });
      }).catch((error) => {
        loadJSON("./app/project/assets/data/translation/", lng).then((responseProject) => {
          output = utilsModule.mergeJSON(output, responseProject);
          langData[lng] = {
            translation: output,
          };
          setLanguage(lng);
        });
      });
    } else {
      setLanguage(lng);
    }
  }

  function initializeDefaultLanguage() {
    return new Promise((resolve, reject) => {
      if (!langData[defaultLng]) {
        let output = {};
        loadJSON("./app/template/assets/data/translation/", defaultLng).then((responseTemplate) => {
          output = utilsModule.mergeJSON(output, responseTemplate);
          loadJSON("./app/project/assets/data/translation/", defaultLng).then((responseProject) => {
            output = utilsModule.mergeJSON(output, responseProject);
            langData[defaultLng] = {
              translation: output,
            };
            setLanguage(defaultLng);
            resolve();
          });
        }).catch((error) => {
          loadJSON("./app/project/assets/data/translation/", defaultLng).then((responseProject) => {
            output = utilsModule.mergeJSON(output, responseProject);
            langData[defaultLng] = {
              translation: output,
            };
            setLanguage(defaultLng);
            resolve();
          });
        });
      } else {
        setLanguage(defaultLng);
        resolve();
      }
    });
  }

  /**
   * 
   * @param {String} keyInput 
   * @param {Object} values 
   */
  function translateInstant(keyInput, values) {
    try {
      return crComLibTranslator.t(keyInput, values);
    } catch (e) {
      return keyInput[0];
    }
  }

  function loadJSON(path, lng) {
    return new Promise((resolve, reject) => {
      serviceModule.loadJSON(path + lng + ".json", (response) => {
        if (response) {
          resolve(JSON.parse(response));
        } else {
          reject("FILE NOT FOUND");
        }
      }, error => {
        reject("FILE NOT FOUND");
      });
    });
  }

  /**
   * Set the language
   * @param {string} lng
   */
  function setLanguage(lng) {
    // update selected language
    crComLibTranslator.changeLanguage(lng);
    setLng = lng;
  }

  /**
   * This is private method to init ch5 i18next translate library
   */
  function initCh5LibTranslate() {
    CrComLib.registerTranslationInterface(crComLibTranslator, "-+", "+-");
    crComLibTranslator.init({
      fallbackLng: "en",
      language: currentLng,
      debug: true,
      resources: langData,
    });
  }

  /**
   * This is public method, it invokes on language change
   * @param {string} lng is language code string like en, fr etc...
   */
  function changeLang(lng) {
    clearTimeout(languageTimer);
    languageTimer = setTimeout(() => {
      if (lng !== defaultLng) {
        defaultLng = lng;
        // invoke language
        getLanguage(lng);
      }
    }, 500);
  }

  /**
   * 
   */
  function getTranslator() {
    return crComLibTranslator;
  }

  /**
   * All public or private methods which need to call on init
   */
  initCh5LibTranslate();

  /**
   * All public method and properties exporting here
   */
  return {
    initializeDefaultLanguage,
    getLanguage,
    changeLang,
    currentLng,
    defaultLng,
    getTranslator,
    translateInstant
  };
})();

// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement 
// under which you licensed this source code.
/*global CrComLib */

const utilsModule = (() => {
  "use strict";

  function log(...input) {
    let allowLogging = true;
    if (allowLogging === true) {
      console.log(...input);
    }
  }

  /**
 * 
 * @param  {...any} args 
 */
  function mergeJSON(...args) {
    let target = {};
    // Merge the object into the target object

    //Loop through each object and conduct a merge
    for (let i = 0; i < args.length; i++) {
      target = merger(target, args[i]);
    }
    return target;
  }

  function merger(target, obj) {
    for (let prop in obj) {
      // eslint-disable-next-line no-prototype-builtins
      if (obj.hasOwnProperty(prop)) {
        if (Object.prototype.toString.call(obj[prop]) === '[object Object]') {
          // If we're doing a deep merge and the property is an object
          target[prop] = mergeJSON(target[prop], obj[prop]);
          // target = merger(target, obj[prop]);
        } else {
          // Otherwise, do a regular merge
          target[prop] = obj[prop];
        }
      }
    }
    return target;
  }

  function dynamicSort(order, ...property) {
    let sort_order = 1;
    if (order === "desc") {
      sort_order = -1;
    }
    return function (a, b) {
      if (property.length > 1) {
        let propA = a[property[0]];
        let propB = b[property[0]];
        for (let i = 1; i < property.length; i++) {
          propA = propA[property[i]];
          propB = propB[property[i]];
        }
        // a should come before b in the sorted order
        if (propA < propB) {
          return -1 * sort_order;
          // a should come after b in the sorted order
        } else if (propA > propB) {
          return 1 * sort_order;
          // a and b are the same
        } else {
          return 0 * sort_order;
        }
      } else {
        // a should come before b in the sorted order
        if (a[property] < b[property]) {
          return -1 * sort_order;
          // a should come after b in the sorted order
        } else if (a[property] > b[property]) {
          return 1 * sort_order;
          // a and b are the same
        } else {
          return 0 * sort_order;
        }
      }
    }
  }

  /**
   * Is object empty
   * @param {object} input 
   */
  function isValidInput(input) {
    if (typeof input === 'number') {
      return true;
    } else if (typeof input === 'string') {
      if (input && input.trim() !== "") {
        return true;
      } else {
        return false;
      }
    } else if (typeof input === 'boolean') {
      return true;
    } else if (typeof input === 'object') {
      if (input) {
        return true;
      } else {
        return false;
      }
    } else if (typeof input === 'undefined') {
      return false;
    } else {
      return false;
    }
  }

  /**
   * Check whether object exists
   * @param {object} input 
   */
  function isValidObject(input) {
    if (!input || input === {} || !isValidInput(input)) {
      return false;
    } else {
      return true;
    }
  }

  /*!
 * Get an object value from a specific path
 * @param  {Object}       obj  The object
 * @param  {String|Array} path The path
 * @param  {*}            def  A default value to return [optional]
 * @return {*}                 The value
 */
  function getContent(obj, path, def) {
    /**
     * If the path is a string, convert it to an array
     * @param  {String|Array} path The path
     * @return {Array}             The path array
     */
    const stringToPath = function (path) {

      // If the path isn't a string, return it
      if (typeof path !== 'string') return path;

      // Create new array
      const output = [];
      // Split to an array with dot notation
      path.split('.').forEach(function (item) {
        // Split to an array with bracket notation
        item.split(/\[([^}]+)\]/g).forEach(function (key) {
          // Push to the new array
          if (key.length > 0) {
            output.push(key);
          }
        });
      });
      return output;
    };

    // Get the path as an array
    path = stringToPath(path);

    // Cache the current object
    let current = obj;

    // For each item in the path, dig into the object
    for (let i = 0; i < path.length; i++) {
      // If the item isn't found, return the default (or null)
      if (!current[path[i]]) return def;
      // Otherwise, update the current  value
      current = current[path[i]];
    }
    return current;
  }

  /*!
   * Replaces placeholders with real content
   * @param {String} template The template string
   * @param {String} local    A local placeholder to use, if any
   */
  function replacePlaceHolders(template, data) {
    // Check if the template is a string or a function
    template = typeof (template) === 'function' ? template() : template;
    if (['string', 'number'].indexOf(typeof template) === -1) throw 'Please provide a valid template';
    // If no data, return template as-is
    if (!data) return template;
    // Replace our curly braces with data
    template = template.replace(/\{\{([^}]+)\}\}/g, function (match) {
      // Remove the wrapping curly braces
      match = match.slice(2, -2);
      // Get the value
      const val = getContent(data, match.trim());
      // Replace
      if (!val) return '{{' + match + '}}';
      return val;
    });
    return template;
  }

  return {
    log,
    dynamicSort,
    isValidObject,
    isValidInput,
    mergeJSON,
    replacePlaceHolders
  };
})();

// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement
// under which you licensed this source code.

/* global WebXPanel, translateModule*/

var webXPanelModule = (function () {
  "use strict";

  const config = {
    "host": window.location.hostname,
    "port": 49200,
    "roomId": "",
    "ipId": "0x03",
    "tokenSource": "",
    "tokenUrl": ""
  };

  const RENDER_STATUS = {
    success: 'success',
    error: 'error',
    warning: 'warning',
    hide: 'hide',
    loading: 'loading'
  };

  var status;
  var pcConfig = config;
  var urlConfig = config;
  var isDisplayHeader = false;
  var isEmptyHeaderComponent = true;
  var isDisplayInfo = false;
  var connectParams = config;
  /**
   * Function to set status bar current state - hidden being default
   * @param {*} classNameToAdd
   */
  function setStatus(classNameToAdd = RENDER_STATUS.hide) {
    if (!isDisplayHeader) {
      return;
    }

    let preloader = document.getElementById('pageStatusIdentifier');
    if (preloader) {
      preloader.className = classNameToAdd;
    }
  }

  /**
   * Get WebXPanel configuration present in project-config.json
   */
  function getWebXPanelConfiguration(projectConfig) {
    if (projectConfig.config && projectConfig.config.controlSystem) {
      pcConfig.host = projectConfig.config.controlSystem.host || config.host;
      pcConfig.port = projectConfig.config.controlSystem.port || config.port;
      pcConfig.roomId = projectConfig.config.controlSystem.roomId || config.roomId;
      pcConfig.ipId = projectConfig.config.controlSystem.ipId || config.ipId;
      pcConfig.tokenSource = projectConfig.config.controlSystem.tokenSource || config.tokenSource;
      pcConfig.tokenUrl = projectConfig.config.controlSystem.tokenUrl || config.tokenUrl;
    }
  }

  /**
   * Convert the URL search params from string to object
   * The key should be in lowercase.
   * @param {object} entries
   * @returns
   */
  function paramsToObject(entries) {
    const result = {}
    for (const [key, value] of entries) {
      result[key.toLowerCase()] = value;
    }
    return result;
  }

  /**
   * Get the url params if defined.
   */
  function getWebXPanelUrlParams() {
    const urlString = window.location.href;
    const urlParams = new URL(urlString);
    const params = new URLSearchParams(urlParams.search);
    const entries = paramsToObject(params);

    // default host should be the IP address of the PC
    urlConfig.host = entries["host"] || pcConfig.host;
    urlConfig.port = entries["port"] || pcConfig.port;
    urlConfig.roomId = entries["roomid"] || pcConfig.roomId;
    urlConfig.ipId = entries["ipid"] || pcConfig.ipId;
    urlConfig.tokenSource = entries["tokensource"] || pcConfig.tokenSource;
    urlConfig.tokenUrl = entries["tokenurl"] || pcConfig.tokenUrl;
  }

  /**
   * Set the listeners for WebXPanel
   */
  function setWebXPanelListeners() {
    // A successful WebSocket connection has been established
    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.CONNECT_WS, (event) => {
      updateInfoStatus("app.webxpanel.status.CONNECT_WS");
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.DISCONNECT_CIP, (msg) => {
      updateInfoStatus("app.webxpanel.status.DISCONNECT_CIP");
      displayConnectionWarning();
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.ERROR_WS, (msg) => {
      updateInfoStatus("app.webxpanel.status.ERROR_WS");
      displayConnectionWarning();
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.AUTHENTICATION_FAILED, (msg) => {
      updateInfoStatus("app.webxpanel.status.AUTHENTICATION_FAILED");
      displayConnectionWarning();
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.AUTHENTICATION_REQUIRED, (msg) => {
      updateInfoStatus("app.webxpanel.status.AUTHENTICATION_REQUIRED");
      displayConnectionWarning();
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.FETCH_TOKEN_FAILED, (msg) => {
      if (msg.detail && msg.status) {
        let statusMsgPrefix = translateModule.translateInstant("app.webxpanel.statusmessageprefix");
        status.innerHTML = statusMsgPrefix + msg.detail.status + " " + msg.detail.statusText;
      } else {
        updateInfoStatus("app.webxpanel.status.FETCH_TOKEN_FAILED");
      }
      displayConnectionWarning();
    });

    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.CONNECT_CIP, (msg) => {
      setStatus(RENDER_STATUS.success);
      removeConnectionWarning();

      // Hide the bar after 10 seconds
      setTimeout(() => {
        setStatus(RENDER_STATUS.hide);
      }, 10000);
      updateInfoStatus("app.webxpanel.status.CONNECT_CIP");

      if (isVersionInfoDisplayed()) {
        document.querySelector('#webxpanel-tab-content .connection .cs').textContent = `CS: wss://${connectParams.host}:${connectParams.port}`;
        document.querySelector('#webxpanel-tab-content .connection .ipid').textContent = `IPID: ${urlConfig.ipId}`;
        if (msg.detail.roomId !== "") {
          document.querySelector('#webxpanel-tab-content .connection .roomid').textContent = `Room Id: ${msg.detail.roomId}`;
        }
      }
    });

    // Authorization
    WebXPanel.default.addEventListener(WebXPanel.WebXPanelEvents.NOT_AUTHORIZED, ({ detail }) => {
      const redirectURL = detail.redirectTo;
      updateInfoStatus("app.webxpanel.status.NOT_AUTHORIZED");

      setTimeout(() => {
        console.log("redirecting to " + redirectURL);
        window.location.replace(redirectURL);
      }, 3000);
    });
  }

  /**
   * Update info status if Info icon is enabled
   */
  function updateInfoStatus(statusMessageKey) {
    let statusMsgPrefix = translateModule.translateInstant("app.webxpanel.statusmessageprefix");
    let statusMessage = translateModule.translateInstant(statusMessageKey);
    if (statusMessage) {
      let sMsg = statusMsgPrefix + statusMessage;
      if (isVersionInfoDisplayed()) {
        status.innerHTML = sMsg;
      } else {
        console.log(sMsg);
      }
    }
  }

  function isVersionInfoDisplayed() {
    return (isDisplayInfo && isEmptyHeaderComponent && isDisplayHeader);
  }
  /**
   * Show the badge on the info icon for connection status.
   */
  function displayConnectionWarning() {
    if (!isVersionInfoDisplayed()) {
      return;
    }

    let classArr = document.getElementById("infobtn").classList;
    if (classArr) {
      classArr.add("warn");
    }
  }

  /**
   * Remove the badge on the info icon.
   */
  function removeConnectionWarning() {
    if (!isVersionInfoDisplayed()) {
      return;
    }

    let classArr = document.getElementById("infobtn").classList;
    if (classArr) {
      classArr.remove("warn");
    }
  }

  /**
   * Show WebXPanel connection status
   */
  function webXPanelConnectionStatus() {
    //Display the connection animation on the header bar
    setStatus(RENDER_STATUS.loading);

    // Hide the animation after 30 seconds
    setTimeout(() => {
      setStatus(RENDER_STATUS.hide);
    }, 30000);
  }


  /**
   * Connect to the control system through websocket connection.
   * Show the status in the header bar using CSS animation.
   * @param {object} projectConfig
   */
  function connectWebXPanel(projectConfig) {
    connectParams = config;

    isDisplayHeader = projectConfig.header.display;
    /**
     * if the header is false then displayInfo needs to be false
     * even if it is set as true in project-config.json
     */
    isDisplayInfo = projectConfig.header.displayInfo;
    isEmptyHeaderComponent = (projectConfig.header.$component === "") ? true : false;

    // Show the connection bar when true
    if (isVersionInfoDisplayed()) {
      status = document.querySelector('#webxpanel-tab-content .connection .status');
    }

    webXPanelConnectionStatus();

    // Merge the configuration params, params of the URL takes precedence
    getWebXPanelConfiguration(projectConfig);
    getWebXPanelUrlParams();

    // Assign the combined configuration
    connectParams = urlConfig;

    WebXPanel.default.initialize(connectParams);

    updateInfoStatus("app.webxpanel.status.CONNECT_WS");

    if (isVersionInfoDisplayed()) {
      if (connectParams.host !== "") {
        document.querySelector('#webxpanel-tab-content .connection .cs').textContent = `CS: wss://${connectParams.host}:${connectParams.port}`;
      }
      if (connectParams.ipId !== "") {
        document.querySelector('#webxpanel-tab-content .connection .ipid').textContent = `IPID: ${Number(connectParams.ipId).toString(16)}`;
      }
      if (connectParams.roomId !== "") {
        document.querySelector('#webxpanel-tab-content .connection .roomid').textContent = `Room Id: ${connectParams.roomId}`;
      }
    }

    // WebXPanel listeners are called in the below method
    setWebXPanelListeners();
  }

  /**
   * Initialize WebXPanel
   */
  function connect(projectConfig) {
    // Connect only in browser environment
    if (typeof WebXPanel !== "undefined" && WebXPanel.isActive) {
      connectWebXPanel(projectConfig);
    } else {
      return;
    }
  }

  /**
   * All public method and properties exporting here
   */
  return {
    connect
  };

})();

let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', (value) => {
    if (value['loaded']) {
        onInit();
        setTimeout(() => {
            CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', loadedSubId);
            loadedSubId = '';
        });
    }
});

function onInit() {

    let lang;

    const languagePack = {
        en: {
            "welcomeMsg": "Welcome to Paris",
            "welcomeHint": "Start the system with this button.",
            "btnStart": "Start",
            "txtToday": "Date",
            "txtTime": "Time",

            "sourceHeader": "Source",
            "sourceHint": "Choose a video source",
            "sofaInputButton": "SOFA INPUT",
            "tvPlayerButton": "TV PLAYER",
            "signageButton": "SIGNAGE",
            "clickShareButton": "CLICK SHARE",
            "yealinkMcoreButton": "YEALINK MCORE",

            "volumeHeader": "Volume",
            "brandMusicHeader": "Brand Music",
            "mediaLevelHeader": "Media Level",

            "channelHeader": "Channels",

            "homeButtonHeader": "Home",

            "displayHeader": "Display",
            "on": "ON",
            "off": "OFF"
        },
        fr: {
            "welcomeMsg": "Bienvenue à Paris",
            "welcomeHint": "Démarrez le système avec ce bouton.",
            "btnStart": "Démarrer",
            "txtToday": "Date",
            "txtTime": "Heure",
            "sourceHeader": "Source",
            "sourceHint": "Choisissez une source vidéo",
            "sofaInputButton": "ENTRÉE CANAPÉ",
            "tvPlayerButton": "LECTEUR TV",
            "signageButton": "PANNEAU PUBLICITAIRE",
            "clickShareButton": "PARTAGE DE CLIC",
            "yealinkMcoreButton": "YEALINK MCORE",
            "volumeHeader": "Volume",
            "brandMusicHeader": "Musique de marque",
            "mediaLevelHeader": "Niveau média",
            "channelHeader": "Channels",
            "homeButtonHeader": "Accueil",
            "displayHeader": "Affichage",
            "on": "ON",
            "off": "OFF"
        }
    };

    // CONSTANTS
    /*
    const btnEnglish = document.getElementById('chooseEnglish');
    const btnFrench = document.getElementById('chooseFrench');
    const imgEnglish = btnEnglish.querySelector('img');
    const imgFrench = btnFrench.querySelector('img');
    */
    const startButton = document.getElementById("startButton");

    // SUBSCRIBE STATES

    CrComLib.subscribeState('b', 'selectLanguage.isEnglishFb', (value) => {
        if (value) {
            lang = 'en';
        } else {
            lang = 'fr';
        }
        changeLangButtonAppearance(value);
        switchLanguage();
    });

    CrComLib.subscribeState('n', 'controlPages.pageFb', (value) => {
        switch (value) {
            case 1:
                templatePageModule.navigateTriggerViewByPageName("page1");
                break;
            case 2:
                templatePageModule.navigateTriggerViewByPageName("settings");
                break;
            case 100:
                templatePageModule.navigateTriggerViewByPageName("fire");
                break;
            default:
                break;
        }
        setTimeout(() => {
            switchLanguage();
        }, 50);
    });


    // PUBLISH EVENTS
/*
    btnEnglish.addEventListener('click', function () {
        CrComLib.publishEvent('b', 'selectLanguage.isEnglish', true);
    });

    btnFrench.addEventListener('click', function () {
        CrComLib.publishEvent('b', 'selectLanguage.isEnglish', false);
    });
*/
    startButton.addEventListener('click', function () {
        startButton.classList.add('active');
        setTimeout(function () {
            CrComLib.publishEvent('n', 'controlPages.page', 2);
            setTimeout(function () {
                startButton.classList.remove('active');
            }, 20);
        }, 20);
    });

/*
    function switchLanguage() {
        const elements = document.querySelectorAll('[data-lang-key]');
        elements.forEach(element => {
            const key = element.getAttribute('data-lang-key');
            element.textContent = languagePack[lang][key];
        });
    }

    // Initially setting English as active
    
    btnEnglish.classList.add('active');

    function changeLangButtonAppearance(isEnglish) {
        if (isEnglish) {
            btnEnglish.classList.add('active');
            imgEnglish.src = "./app/template/assets/img/translate-black.svg";
            btnFrench.classList.remove('active');
            imgFrench.src = "./app/template/assets/img/translate.svg";
        } else {
            btnFrench.classList.add('active');
            imgFrench.src = "./app/template/assets/img/translate-black.svg";
            btnEnglish.classList.remove('active');
            imgEnglish.src = "./app/template/assets/img/translate.svg";
        }
    }
    */
}

function showAlert(message, duration) {
    const alertBox = document.getElementById('customAlert');
    alertBox.innerText = message;
    alertBox.style.display = 'block'; // Show the alert

    // Hide the alert after the specified duration
    setTimeout(() => {
        alertBox.style.display = 'none';
    }, duration);
}
/*jslint es6 */
/*global CrComLib, webXPanelModule, hardButtonsModule, templateVersionInfoModule, projectConfigModule, featureModule, templateAppLoaderModule, translateModule, serviceModule, utilsModule, navigationModule */

const templatePageModule = (() => {
	'use strict';

	let triggerview = null;
	let horizontalMenuSwiperThumb = null;
	let selectedPage = { name: "" };
	let totalPreloadPage = 0;
	let preloadPageLoaded = 0;
	let _isPageLoaded = false;
	let firstLoad = false;
	let pageLoadTimeout = 2000;
	let isWebXPanelInitialized = false; // avoid calling connection method multiple times

	const effects = {
		"fadeOutUpBig": ["animate__animated", "animate__fadeOutUpBig"],
		"fadeInUpBig": ["animate__animated", "animate__fadeInUpBig"],
		"fadeOutDownBig": ["animate__animated", "animate__fadeOutDownBig"],
		"fadeInDownBig": ["animate__animated", "animate__fadeInDownBig"],
		"fadeOutUpBigFast": ["animate__animated", "animate__fadeOutUpBig", "animate__fast"],
		"fadeInUpBigFast": ["animate__animated", "animate__fadeInUpBig", "animate__fast"],
		"fadeOutDownBigFast": ["animate__animated", "animate__fadeOutDownBig", "animate__fast"],
		"fadeInDownBigFast": ["animate__animated", "animate__fadeInDownBig", "animate__fast"],
		"fadeOut": ["animate__animated", "animate__fadeOut"],
		"fadeOutSlow": ["animate__animated", "animate__fadeOut", "animate__slow"],
		"fadeIn": ["animate__animated", "animate__fadeIn"],
		"fadeInSlow": ["animate__animated", "animate__fadeIn", "animate__slow"],
		"fadeInFast": ["animate__animated", "animate__fadeIn", "animate__fast"],
		"zoomIn": ["animate__animated", "animate__zoomIn"],
		"zoomOut": ["animate__animated", "animate__zoomOut"],
		"fadeOutFast": ["animate__animated", "animate__fadeOut", "animate__fast"]
	};

	/**
	 * This is public method for bottom navigation to navigate to next page
	 * @param {number} idx is selected index for navigate to appropriate page
	 */
	function navigateTriggerViewByPageName(pageName) {
		// If the previous and selected page are same then exit
		if (pageName !== selectedPage.pageName) {
			const pageObject = projectConfigModule.getNavigationPages().find(page => page.pageName === pageName);
			const oldPage = JSON.parse(JSON.stringify(selectedPage));
			// Loop and set url and receiveStateUrl based on proper preload and cachePage values
			if (oldPage.preloadPage === true && oldPage.cachePage === false) {
				const htmlImportSnippet = document.getElementById(oldPage.pageName + "-import-page");
				htmlImportSnippet.removeAttribute("url");
				htmlImportSnippet.setAttribute("receiveStateShow", oldPage.pageName + "-import-page-show");
				htmlImportSnippet.setAttribute("noShowType", "remove");
			} else if (oldPage.preloadPage === false && oldPage.cachePage === true) {
				const htmlImportSnippet = document.getElementById(oldPage.pageName + "-import-page");
				htmlImportSnippet.removeAttribute("receiveStateShow");
				if (htmlImportSnippet.hasAttribute("url") === false || !htmlImportSnippet.getAttribute("url") || htmlImportSnippet.getAttribute("url") === "") {
					htmlImportSnippet.setAttribute("url", oldPage.fullPath + oldPage.fileName);
				}
				htmlImportSnippet.setAttribute("noShowType", "display");
			}
			CrComLib.publishEvent("b", "active_state_class_" + oldPage.pageName, false);
			selectedPage = JSON.parse(JSON.stringify(pageObject));
			CrComLib.publishEvent("b", "active_state_class_" + selectedPage.pageName, true);
			if (triggerview !== null) {
				const activeIndex = projectConfigModule.getNavigationPages().findIndex(data => data.pageName === pageName);
				try {
					// menuMoveInViewPort();

					if (projectConfigModule.getMenuOrientation() === "horizontal" || projectConfigModule.getMenuOrientation() === "vertical") {
						let intersectionOptions = {
							rootMargin: '0px',
							threshold: 1.0
						};
						const intersectionObserver = new IntersectionObserver((entries, observer) => {
							entries.forEach(entry => {
								if (entry.isIntersecting === false) {
									CrComLib.publishEvent("n", "scrollToMenu", activeIndex);
								}
							});
							intersectionObserver.unobserve(document.getElementById('menu-list-id-' + activeIndex));
						}, intersectionOptions);
						intersectionObserver.observe(document.getElementById('menu-list-id-' + activeIndex));
						// intersectionObserver.unobserve(document.getElementById('menu-list-id-' + activeIndex));
					}
					triggerview.setActiveView(activeIndex);
				} catch (e) {
					console.error(e);
				}
			}
			navigationModule.goToPage(pageName);
		}
	}

	function menuMoveInViewPort() {
		// 	// TODO: Subscribe and unsubscribe to avoid unwanted scrolls
		// 	// if (response.menuOrientation === 'horizontal') { // || response.menuOrientation === 'vertical') {
		// CrComLib.subscribeInViewPortChange(document.getElementById('menu-list-id-' + activeIndex), (element, isInViewPort) => {
		// 	if (!isInViewPort) {
		// 		console.log("Publishing now", activeIndex);
		// 		CrComLib.publishEvent("n", "scrollToMenu", activeIndex);
		// 	}
		// 	// setTimeout(() => {
		// 	CrComLib.unSubscribeInViewPortChange(document.getElementById('menu-list-id-' + activeIndex));
		// 	// });
		// });
	}

	function setMenuActive() {
		// if (triggerview !== null) {
		// 	if (response.menuOrientation === 'horizontal') { // || response.menuOrientation === 'vertical') {
		// 		CrComLib.publishEvent("n", "scrollToMenu", activeIndex);
		// 	}
		// }
	}

	function navigateTriggerViewByIndex(index) {
		const listOfPages = projectConfigModule.getNavigationPages();
		if (listOfPages.length > 0 && index >= 0 && index <= listOfPages.length) {
			navigateTriggerViewByPageName(listOfPages[index].pageName);
		}
	}

	function isPageLoaded() {
		return _isPageLoaded;
	}

	/**
	 * This is public method to show/hide bottom navigation in smaller screen
	 */
	function openThumbNav() {
		const horizontalMenuSwiperThumb = document.getElementById("horizontal-menu-swiper-thumb");
		horizontalMenuSwiperThumb.className += " open";
		event.stopPropagation();
	}

	/**
	 * This is public method to toggle left navigation sidebar
	 */
	function toggleSidebar() {
		let sidebarToggle = document.getElementById("sidebarToggle");
		if (sidebarToggle) {
			sidebarToggle.classList.toggle("active");
		}
		let navbarThumb = document.querySelector(".swiper-thumb");
		if (navbarThumb) {
			navbarThumb.classList.toggle("open");
		}
	}

	/**
	 * This method will invoke on body click
	 */
	document.body.addEventListener("click", function (event) {
		triggerview = document.querySelector(".triggerview");
		horizontalMenuSwiperThumb = document.getElementById("horizontal-menu-swiper-thumb");

		if (event.target.id === "sidebarToggle") {
			event.stopPropagation();
		} else {
			let navbarThumb = document.querySelector(".swiper-thumb");
			if (navbarThumb) {
				navbarThumb.classList.remove("open");
			}
			if (horizontalMenuSwiperThumb) {
				horizontalMenuSwiperThumb.classList.remove("open");
			}
			let sidebarToggle = document.getElementById("sidebarToggle");
			if (sidebarToggle) {
				sidebarToggle.classList.remove("active");
			}
		}
	});

	/**
	 * Load the emulator, theme, default language and listeners
	 */
	let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:template-page-import-page', (value) => {
		if (value['loaded']) {
			triggerview = document.querySelector(".triggerview");
			horizontalMenuSwiperThumb = document.getElementById("horizontal-menu-swiper-thumb");

			projectConfigModule.projectConfigData().then((projectConfigResponse) => {
				translateModule.initializeDefaultLanguage().then(() => {
					featureModule.changeTheme();
					/* Note: You can uncomment below line to enable remote logger.
					* Refer below documentation link to know more about remote logger.
					* https://sdkcon78221.crestron.com/sdk/Crestron_HTML5UI/Content/Topics/UI-Remote-Logger.htm
					*/
					// featureModule.initializeLogger(serverIPAddress, serverPortNumber);
					serviceModule.initialize(projectConfigResponse);
					// navigationModule.goToPage(projectConfigResponse.content.$defaultView);
					featureModule.logDiagnostics(projectConfigResponse.header.diagnostics.logs.logDiagnostics);

					// Changes for index.html - Start
					document.getElementById("favicon").setAttribute("href", projectConfigResponse.faviconPath);
					const getSelectedTheme = projectConfigResponse.themes.find(themeName => themeName.name === projectConfigResponse.selectedTheme);
					if (getSelectedTheme) {
						document.getElementById("selectedThemeCss").setAttribute("href", "./assets/css/" + getSelectedTheme.extends + ".css");
					}

					const widgetsAndStandalonePages = document.getElementById("widgets-and-standalone-pages");
					const widgets = projectConfigResponse.content.widgets;
					for (let i = 0; i < widgets.length; i++) {
						const htmlImportSnippet = document.createElement("ch5-import-htmlsnippet");
						htmlImportSnippet.setAttribute("id", widgets[i].widgetName + "-import-widget");
						htmlImportSnippet.setAttribute("url", widgets[i].fullPath + widgets[i].fileName);
						htmlImportSnippet.setAttribute("show", "false");
						widgetsAndStandalonePages.appendChild(htmlImportSnippet);
					}

					const standAlonePages = projectConfigModule.getAllStandAloneViewPages();
					for (let i = 0; i < standAlonePages.length; i++) {
						const htmlImportSnippet = document.createElement("ch5-import-htmlsnippet");
						htmlImportSnippet.setAttribute("id", standAlonePages[i].pageName + "-import-page");
						htmlImportSnippet.setAttribute("url", standAlonePages[i].fullPath + standAlonePages[i].fileName);
						htmlImportSnippet.setAttribute("show", "false");
						widgetsAndStandalonePages.appendChild(htmlImportSnippet);
					}
					// Changes for index.html - End

					// Header
					if (projectConfigResponse.header.display === true) {
						let dataHeader = "";
						if (projectConfigResponse.header.$component && projectConfigResponse.header.$component !== "") {
							dataHeader = document.getElementById("header-section-page-template2").innerHTML;
						} else {
							dataHeader = document.getElementById("header-section-page-template1").innerHTML;
						}

						const app = document.getElementById('header-section-page');
						const mergedJsonContentHeader = utilsModule.mergeJSON(projectConfigResponse, {
							customHeaderUrl: projectConfigModule.getCustomHeaderUrl()
						});
						app.innerHTML = utilsModule.replacePlaceHolders(dataHeader, mergedJsonContentHeader);

						let sidebarToggle = document.getElementById("sidebarToggle");
						if (projectConfigResponse.menuOrientation === "vertical") {
							if (sidebarToggle) {
								sidebarToggle.classList.remove("display-none");
							}
						} else {
							if (sidebarToggle) {
								if (!sidebarToggle.classList.contains("display-none")) {
									sidebarToggle.classList.add("display-none");
								}
							}
						}

						if (document.getElementById("brandLogo")) {
							const sTheme = projectConfigResponse.selectedTheme;
							const themes = projectConfigResponse.themes;
							themes.forEach((elem) => {
								if (sTheme === elem.name) {
									if (elem.brandLogo !== "undefined") {
										for (var prop in elem.brandLogo) {
											if (elem.brandLogo[prop] !== "") {
												document.getElementById("brandLogo").setAttribute(prop, elem.brandLogo[prop]);
											}
										}
									}
								}
							});
						}

						if (projectConfigResponse.header.displayInfo === true) {
							if (projectConfigResponse.header.$component === "") {
								const headerSectionPageSet1 = document.getElementById('header-section-page-set1');
								headerSectionPageSet1.innerHTML = utilsModule.replacePlaceHolders(document.getElementById("header-section-page-template1-set1").innerHTML, mergedJsonContentHeader);
							}
						}
					} else {
						document.getElementById("header-index-page").remove();
					}

					// Content
					const app = document.getElementById('template-content-page-content');
					let data = "";
					if (projectConfigResponse.menuOrientation === "horizontal") {
						data = document.getElementById("template-content-page-section-horizontal").innerHTML;
					} else if (projectConfigResponse.menuOrientation === "vertical") {
						data = document.getElementById("template-content-page-section-vertical").innerHTML;
					} else {
						data = document.getElementById("template-content-page-section-none").innerHTML;
					}

					const templateContentBackground = document.getElementById("template-content-background");
					if (templateContentBackground) {
						const sTheme = projectConfigResponse.selectedTheme;
						const themes = projectConfigResponse.themes;
						themes.forEach((elem) => {
							if (sTheme === elem.name) {
								if (elem.backgroundProperties !== "undefined") {
									for (let prop in elem.backgroundProperties) {

										if (prop === "url") {
											if (typeof elem.backgroundProperties.url === "object") {
												elem.backgroundProperties.url = elem.backgroundProperties.url.join(" | ");
											}
										}
										if (prop === "backgroundColor") {
											if (typeof elem.backgroundProperties.backgroundColor === "object") {
												elem.backgroundProperties.backgroundColor = elem.backgroundProperties.backgroundColor.join(' | ');
											}
										}

										if (elem.backgroundProperties[prop] !== "") {
											templateContentBackground.setAttribute(prop, elem.backgroundProperties[prop]);
										}
									}
								}
							}
						});
					}
					const mergedJsonContent = utilsModule.mergeJSON(projectConfigResponse, {});
					app.innerHTML = utilsModule.replacePlaceHolders(data, mergedJsonContent);

					const pagesList = projectConfigModule.getNavigationPages();
					pagesList.forEach(e => { if (e.preloadPage) totalPreloadPage++ })
					if (projectConfigResponse.menuOrientation === "horizontal") {
						const horizontalMenuSwiperThumb = document.getElementById("horizontal-menu-swiper-thumb");
						if (horizontalMenuSwiperThumb) {
							horizontalMenuSwiperThumb.setAttribute("size", pagesList.length);
						}
					} else if (projectConfigResponse.menuOrientation === "vertical") {
						const verticalMenuSwiperThumb = document.getElementById("vertical-menu-swiper-thumb");
						if (verticalMenuSwiperThumb) {
							verticalMenuSwiperThumb.setAttribute("size", pagesList.length);
						}
					}

					let triggerviewInContent = "";
					if (projectConfigResponse.menuOrientation === "horizontal") {
						triggerviewInContent = document.getElementById("triggerviewInContentHorizontal");
					} else if (projectConfigResponse.menuOrientation === "vertical") {
						triggerviewInContent = document.getElementById("triggerviewInContentVertical");
					} else {
						triggerviewInContent = document.getElementById("triggerviewInContentNone");
					}
					if (triggerviewInContent) {
						const tgViewProperties = projectConfigResponse.content.triggerViewProperties;
						if (tgViewProperties) {
							Object.entries(tgViewProperties).forEach(([key, value]) => {
								triggerviewInContent.setAttribute(key, value);
							});
						}

						for (let i = 0; i < pagesList.length; i++) {
							const childNodeTriggerView = document.createElement("ch5-triggerview-child");
							const tgViewChildProperties = projectConfigResponse.content.pages[i].triggerViewChildProperties;
							if (tgViewChildProperties) {
								Object.entries(tgViewChildProperties).forEach(([key, value]) => {
									childNodeTriggerView.setAttribute(key, value);
								});
							}

							/*
							// LOADING INDICATOR - Uncomment the below lines along with code in navigation.js file to enable loading indicator
							const htmlImportSnippetForLoader = document.createElement("ch5-import-htmlsnippet");
							htmlImportSnippetForLoader.setAttribute("id", pagesList[i].pageName + "-import-page-app-loader");
							htmlImportSnippetForLoader.setAttribute("receiveStateShow", pagesList[i].pageName + "-import-page-show-app-loader");
							htmlImportSnippetForLoader.setAttribute("url", "./app/template/components/widgets/template-app-loader/template-app-loader.html");							
							*/

							const htmlImportSnippet = document.createElement("ch5-import-htmlsnippet");
							htmlImportSnippet.setAttribute("id", pagesList[i].pageName + "-import-page");

							/*
							preloadPage: FALSE + cachedPage: FALSE (Default setting)
								* page is not loaded on startup - load time is only during first time page is called
								* page is not cached - each time user comes to the page, the page is loaded, and unloaded when user leaves the page.
							preloadPage: FALSE + cachedPage: TRUE
								* page is not loaded on startup - load time is only during the time page is called. Since page is cached, load time is only for first time.
								* page is cached - load time is whenever the user opens the page. Each time user comes to the page, the page is available already and there is no page load time. Even after user leaves the page, the page is not removed from DOM and is always available. DOM weight for project is high because of this feature.
							preloadPage: TRUE + cachedPage: FALSE
								* page is loaded on startup - load time is during first time page is called
								* page is not cached - each time user comes to the page, the page is loaded, and unloaded when user leaves the page. However, since the page is loaded for first time, the page will not be removed from DOM unless user visits the page atleast once. Once the user visits the page, and leaves the page, the page is removed from DOM. After user leaves the page, the load time is during each page call again.
							preloadPage: TRUE + cachedPage: TRUE
								* page is loaded on startup - load time is during first time page is called
								* page is cached - load time is during the project load. Each time user comes to the page, the page is available already and there is no page load time. Even after user leaves the page, the page is not removed from DOM and is always available. DOM weight for project is high because of this feature.
							*/
							if (CrComLib.isCrestronTouchscreen()) {
								pageLoadTimeout = 15000;
							}

							if (pagesList[i].preloadPage === true) {
								// We need the below becos there is a flicker when page loads and hides if url is set - specifically with signal sent
								setTimeout(() => {
									htmlImportSnippet.setAttribute("url", pagesList[i].fullPath + pagesList[i].fileName);
									preloadPageLoaded++;
								}, pageLoadTimeout);
								htmlImportSnippet.setAttribute("noShowType", "display");
							} else {
								htmlImportSnippet.setAttribute("receiveStateShow", pagesList[i].pageName + "-import-page-show");
								if (pagesList[i].cachePage === true) {
									htmlImportSnippet.setAttribute("noShowType", "display");
								} else {
									htmlImportSnippet.setAttribute("noShowType", "remove");
								}
							}

							// LOADING INDICATOR - Uncomment the below line along with code in navigation.js file to enable loading indicator
							// childNodeTriggerView.appendChild(htmlImportSnippetForLoader);
							childNodeTriggerView.appendChild(htmlImportSnippet);
							triggerviewInContent.appendChild(childNodeTriggerView);
						}
						triggerviewInContent.setAttribute("activeview", projectConfigModule.defaultActiveViewIndex());
						triggerview = triggerviewInContent;
					}

					// Footer
					if (projectConfigResponse.footer.display === true) {
						const appFooter = document.getElementById('footer-section-page');
						let dataFooter = "";
						if (projectConfigResponse.footer.$component && projectConfigResponse.footer.$component !== "") {
							dataFooter = document.getElementById("footer-section-page-template2").innerHTML;
						} else {
							dataFooter = document.getElementById("footer-section-page-template1").innerHTML;
						}

						const mergedJsonContentFooter = utilsModule.mergeJSON(projectConfigResponse, {
							copyrightYear: (new Date()).getFullYear(),
							customFooterUrl: projectConfigModule.getCustomFooterUrl()
						});
						appFooter.innerHTML = utilsModule.replacePlaceHolders(dataFooter, mergedJsonContentFooter);
					} else {
						document.getElementById("footer-index-page").remove();
					}

					if (triggerview) {
						triggerview.addEventListener("select", (event) => {
							const listOfPages = projectConfigModule.getNavigationPages();
							if (listOfPages.length > 0 && event.detail !== undefined && listOfPages[event.detail].pageName !== selectedPage.pageName) {
								navigateTriggerViewByIndex(event.detail);
							}
						});
					}

					CrComLib.subscribeState('s', 'Csig.Product_Name_Text_Join_fb', (deviceSpecificData) => {
						hardButtonsModule.initialize(deviceSpecificData).then(hardButtonResponse => {
							let responseArrayForNavPages = projectConfigModule.getNavigationPages();
							if (projectConfigResponse.menuOrientation === "horizontal") {

								// window.customElements.whenDefined('horizontal-menu-swiper-thumb').then(()=>{

								let loadListCh5 = CrComLib.subscribeState('o', 'ch5-list', (value) => {
									if (value['loaded'] && (value['id'] === "horizontal-menu-swiper-thumb")) {
										loadCh5ListForMenu(projectConfigResponse, responseArrayForNavPages);
										connectToWebXPanel(projectConfigResponse);
										navigateToFirstPage(projectConfigResponse, responseArrayForNavPages);
										setTimeout(() => {
											CrComLib.unsubscribeState('o', 'ch5-list', loadListCh5);
											loadListCh5 = null;
										});
									}
								});
							} else if (projectConfigResponse.menuOrientation === "vertical") {
								let loadListCh5 = CrComLib.subscribeState('o', 'ch5-list', (value) => {
									if (value['loaded'] && (value['id'] === "vertical-menu-swiper-thumb")) {
										loadCh5ListForMenu(projectConfigResponse, responseArrayForNavPages);
										connectToWebXPanel(projectConfigResponse);
										navigateToFirstPage(projectConfigResponse, responseArrayForNavPages);
										setTimeout(() => {
											CrComLib.unsubscribeState('o', 'ch5-list', loadListCh5);
											loadListCh5 = null;
										});
									}
								});
							} else {
								connectToWebXPanel(projectConfigResponse);
								navigateToFirstPage(projectConfigResponse, responseArrayForNavPages);
							}
						});
					});

				});
			});

			setTimeout(() => {
				CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:template-page-import-page', loadedSubId);
				loadedSubId = null;
			});
		}
	});

	function setTransition(app) {
		const selectedEffect = effects.fadeIn;
		for (let i = 0; i < selectedEffect.length; i++) {
			app.classList.add(selectedEffect[i]);
		}
	}

	function connectToWebXPanel(projectConfigResponse) {
		if (projectConfigResponse.useWebXPanel && !isWebXPanelInitialized) {
			if (projectConfigResponse.header.display && projectConfigResponse.header.displayInfo && projectConfigResponse.header.$component.trim() === "") {
				let loadListCh5 = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:template-version-info-import-page', (value) => {
					if (value['loaded']) {
						webXPanelModule.connect(projectConfigResponse);
						isWebXPanelInitialized = true;
						setTimeout(() => {
							CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:template-version-info-import-page', loadListCh5);
							loadListCh5 = null;
						});
					}
				});
			} else {
				webXPanelModule.connect(projectConfigResponse);
				isWebXPanelInitialized = true;
			}
		}
	}

	function loadCh5ListForMenu(projectConfigResponse, responseArrayForNavPages) {
		for (let i = 0; i < responseArrayForNavPages.length; i++) {
			const menu = document.getElementById("menu-list-id-" + i);
			if (menu) {
				if (responseArrayForNavPages[i].navigation.iconUrl && responseArrayForNavPages[i].navigation.iconUrl !== "") {
					menu.setAttribute("iconUrl", responseArrayForNavPages[i].navigation.iconUrl);
				} else if (responseArrayForNavPages[i].navigation.iconClass && responseArrayForNavPages[i].navigation.iconClass !== "") {
					menu.setAttribute("iconClass", responseArrayForNavPages[i].navigation.iconClass);
				}
				if (responseArrayForNavPages[i].navigation.isI18nLabel === true) {
					menu.setAttribute("label", translateModule.translateInstant(responseArrayForNavPages[i].navigation.label));
				} else {
					menu.setAttribute("label", responseArrayForNavPages[i].navigation.label);
				}
				menu.setAttribute("iconClass", responseArrayForNavPages[i].navigation.iconClass);
				if (projectConfigResponse.menuOrientation === 'horizontal') {
					menu.setAttribute("iconPosition", responseArrayForNavPages[i].navigation.iconPosition);
				}
				menu.setAttribute("receiveStateSelected", "active_state_class_" + responseArrayForNavPages[i].pageName);
				menu.setAttribute("onRelease", "templatePageModule.navigateTriggerViewByPageName('" + responseArrayForNavPages[i].pageName + "')");
			}
		}
	}

	function navigateToFirstPage(projectConfigResponse, responseArrayForNavPages) {
		let newIndex = -99;
		if (projectConfigResponse.content.$defaultView && projectConfigResponse.content.$defaultView !== "") {
			for (let i = 0; i < responseArrayForNavPages.length; i++) {
				if (responseArrayForNavPages[i].pageName.toString().trim().toUpperCase() === projectConfigResponse.content.$defaultView.toString().trim().toUpperCase()) {
					newIndex = i;
					break;
				} else {
					newIndex = -1;
				}
			}
		} else {
			newIndex = 0;
		}

		if (newIndex === -99 || newIndex === -1) {
			newIndex = 0;
		}
		navigateTriggerViewByIndex(newIndex);
		_isPageLoaded = true;
	}

	/**
	 * Loader method is for spinner
	 */
	function hideLoading(pageObject) {
		if (totalPreloadPage === preloadPageLoaded) {
			if (!firstLoad && totalPreloadPage !== 0) {
				firstLoad = true;
				const listOfPages = projectConfigModule.getNavigationPages();
				setTimeout(() => {
					listOfPages.forEach((page) => page.preloadPage ? navigationModule.updateDiagnosticsOnPageChange(page.pageName) : '');
				}, pageLoadTimeout);

			}
			document.getElementById("loader").style.display = "none";
		} else {
			setTimeout(() => {
				hideLoading(pageObject);
			}, 500);
		}
	}

	window.addEventListener("orientationchange", function () {
		try {
			templatePageModule.setMenuActive();
		} catch (e) {
			// console.log(e);
		}
	}, false);

	/**
	 * All public method and properties exporting here
	 */
	return {
		navigateTriggerViewByPageName,
		isPageLoaded,
		openThumbNav,
		toggleSidebar,
		hideLoading,
		navigateTriggerViewByIndex,
		setTransition
	};

})();

/*jslint es6 */
/*global CrComLib, projectConfigModule, translateModule, serviceModule, utilsModule, templateAppLoaderModule */

const templateAppLoaderModule = (() => {
	'use strict';

	function isCachePageLoaded(routeId) {
		if (document.getElementById(routeId)) {
			return document.getElementById(routeId).hasAttribute("url") &&
				document.getElementById(routeId).getAttribute("url") !== null &&
				document.getElementById(routeId).getAttribute("url") !== undefined &&
				document.getElementById(routeId).getAttribute("url") !== "";
		} else {
			return false;
		}
	}

	function showLoading(pageObject) {
		const routeId = pageObject.pageName + "-import-page";
		const isCached = isCachePageLoaded(routeId);
		if (isCached === false) {
			CrComLib.publishEvent('b', routeId + '-show-app-loader', true);
		}
	}

	/**
	 * All public method and properties are exported here
	 */
	return {
		showLoading,
		isCachePageLoaded
	};

})();
// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement
// under which you licensed this source code.
/*jslint es6 */
/*global CrComLib, translateModule, serviceModule, utilsModule, templatePageModule */

const templateRemoteLoggerSettingsModule = (() => {
  "use strict";

  let logger;
  let isConfigured = false;
  let appender = {};
  let clickCount = 0;
  let startTimer = 0;
  let ds = null;
  let dsElem = null;
  let rlbtn = null;
  let errorMessage = null;
  let ipAddressElem = null;
  let portNumberElem = null;

  function onInit() {
    ds = document.getElementById("template-dstatus");
    dsElem = document.getElementsByClassName('dockerstatus');
    rlbtn = document.getElementById('template-rlbtn');
    errorMessage = document.querySelector(".ui.error.message");
    ipAddressElem = document.getElementById("loggerIpAddress");
    portNumberElem = document.getElementById("loggerPortNumber");
  }

  /**
   * Reset Status
   */
  function resetStatus() {
    ds.innerHTML = translateModule.translateInstant("app.ch5logger.docker.dockerdisconnected");
    dsElem[0].firstChild.classList.remove("red");
    dsElem[0].firstChild.classList.remove("amber");
    dsElem[0].firstChild.classList.remove("green");
  }

  /**
   * Reset the connection style
   */
  function resetConnection() {
    const errorMessage = document.querySelector(".ui.error.message");
    errorMessage.style.display = "none";
    resetStatus();
    if (logger !== undefined) {
      logger.disconnect();
    }
    disconnect();
  }

  /**
   * Perform actions related to remote logger disconection
   * and set the values for connect
   */
  function disconnect() {
    rlbtn.disabled = false;
    rlbtn.className = "connect";
    ipAddressElem.disabled = false;
    portNumberElem.disabled = false;
    if (logger !== undefined) {
      logger.disconnect();
    }
    rlbtn.innerHTML = translateModule.translateInstant("app.ch5logger.form.connect");
  }

  /**
   * Perform actions related to remote logger disconection
   * and set the values for disconnect
   */
  function connect() {
    rlbtn.disabled = false;
    ipAddressElem.disabled = true;
    portNumberElem.disabled = true;
    rlbtn.className = "disconnect";
    rlbtn.innerHTML = translateModule.translateInstant("app.ch5logger.form.disconnect");
  }

  /**
   * Set the remote logger configuration for docker
   */
  function setRemoteLoggerConfig(hName, pNumber) {
    try {
      // Store hostname and port number
      ipAddressElem.disabled = true;
      portNumberElem.disabled = true;
      rlbtn.disabled = true;

      if (isConfigured) {
        appender.resetIP(hName, pNumber);
        logger = CrComLib.getLogger(appender, true);
      } else {
        appender = CrComLib.getRemoteAppender(hName, pNumber);
        logger = CrComLib.getLogger(appender, true);
        isConfigured = true;

        logger.subscribeDockerStatus.subscribe((message) => {
          if (message !== "") {
            resetStatus();
            if (message === "DOCKER_CONNECTING") {
              rlbtn.innerHTML = translateModule.translateInstant("app.ch5logger.form.connecting");
              dsElem[0].firstChild.classList.add("amber");
            } else if (message === "DOCKER_CONNECTED") {
              connect();
              dsElem[0].firstChild.classList.add("green");
            } else if (message === "DOCKER_ERROR") {
              disconnect();
              dsElem[0].firstChild.classList.add("red");
            }
            message = message.toLowerCase();
            message = message.replace(/_/, "");
            ds.innerHTML = translateModule.translateInstant("app.ch5logger.docker." + message);
          }
        });
      }
    } catch (error) {
      ipAddressElem.disabled = false;
      portNumberElem.disabled = false;
      rlbtn.disabled = false;
      utilsModule.log(error);
    }
  }

  /**
   * Counts the clicks happened in the time difference
   */
  function clickCounter() {
    if (startTimer) {
      if (timeDifference() > 3) {
        resetTimer();
      }
    }
    clickCount += 1;
    if (clickCount == 1) {
      startTimer = Date.now();
    }
  }

  /**
   * Reset the time
   */
  function resetTimer() {
    clickCount = 0;
    startTimer = 0;
  }

  /**
   * Calculate the Time difference
   */
  function timeDifference() {
    const endTimer = Date.now();
    const timerDiff = Math.floor((endTimer - startTimer) / 1000);
    return timerDiff;
  }

  /**
   * Displays the logger popup
   */
  function showLoggerPopUp() {
    const model = document.getElementById("loggerModalWrapper");
    const errorMessage = document.querySelector(".ui.error.message");
    errorMessage.style.display = "none";
    clickCounter();
    if (clickCount === 5) {
      if (timeDifference() <= 3) {
        CrComLib.publishEvent("b", "template-remote-logger.clicked", true);
        model.style.display = "block";
        resetTimer();
      } else {
        CrComLib.publishEvent("b", "template-remote-logger.clicked", false);
        model.style.display = "none";
        resetTimer();
      }
    }
  }

  /**
   * Retrieve the inputs from the form and passes to the setRemoteLoggerConfig()
   */
  function updateLoggerInfo() {
    const hostName = ipAddressElem.value;
    const portNumber = portNumberElem.value;
    if (rlbtn.classList.contains("connect")) {
      setRemoteLoggerConfig(hostName, portNumber);
    } else {
      resetConnection();
    }
  }

  /**
   * Validate the IP Address / Hostname and Port number provided in the form
   */
  function validate() {
    let ipExp = /^(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))$/;
    let hostExp = /^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9-]*[A-Za-z0-9])$/;
    errorMessage.style.display = "none";
    let ip = false;
    let port = false;
    errorMessage.innerHTML = "";
    if (ipAddressElem.value === "" || ipAddressElem.value === undefined || ipAddressElem.value === null) {
      errorMessage.innerHTML = "Please enter IP Address/Hostname";
      errorMessage.style.display = "block";
      return false;
    }
    if (portNumberElem.value === "" || portNumberElem.value === undefined || portNumberElem.value === null) {
      errorMessage.innerHTML = "Please enter Port Number";
      errorMessage.style.display = "block";
      return false;
    }
    if (
      ipAddressElem.value !== undefined &&
      ipAddressElem.value !== null &&
      ipAddressElem.value !== "0.0.0.0" &&
      ipAddressElem.value !== "255.255.255.255" &&
      ipAddressElem.value.length <= 127 &&
      (ipExp.test(ipAddressElem.value) || hostExp.test(ipAddressElem.value))
    ) {
      ip = true;
      errorMessage.style.display = "none";
    } else {
      errorMessage.innerHTML = "Please enter valid IP Address/Hostname";
      errorMessage.style.display = "block";
      return false;
    }
    if (
      portNumberElem.value !== null &&
      !isNaN(portNumberElem.value) &&
      portNumberElem.value >= 1025 &&
      portNumberElem.value < 65536
    ) {
      port = true;
      errorMessage.style.display = "none";
    } else {
      errorMessage.innerHTML = "Please enter valid Port Number between 1025 to 65536";
      errorMessage.style.display = "block";
      return false;
    }
    if (ip && port) {
      errorMessage.style.display = "none";
      updateLoggerInfo();
    }
  }

  /**
 * private method for page class initialization
 */
  let loadedImportSnippet = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:template-remote-logger-settings-import-page', (value) => {
    if (value['loaded']) {
      setTimeout(() => {
        onInit();
      }, 5000);
      setTimeout(() => {
        CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:template-remote-logger-settings-import-page', loadedImportSnippet);
        loadedImportSnippet = null;
      });
    }
  });

  /**
   * All public method and properties are exported here
   */
  return {
    showLoggerPopUp: showLoggerPopUp,
    validate: validate,
    resetConnection: resetConnection,
    updateLoggerInfo: updateLoggerInfo,
    setRemoteLoggerConfig: setRemoteLoggerConfig,
  };
})();
// Copyright (C) 2022 to the present, Crestron Electronics, Inc.
// All rights reserved.
// No part of this software may be reproduced in any form, machine
// or natural, without the express written consent of Crestron Electronics.
// Use of this source code is subject to the terms of the Crestron Software License Agreement
// under which you licensed this source code.

/*global CrComLib, translateModule, serviceModule, utilsModule, templateAppLoaderModule, templatePageModule, projectConfigModule, projectConfigModule */

const templateVersionInfoModule = (() => {
	'use strict';

	let projectConfig;
	const tableCount = {};
	const componentCount = {};
	/**
	 * Initialize Method
	 */
	function onInit() {
		projectConfigModule.projectConfigData().then(projectConfigResponse => {
			projectConfig = projectConfigResponse;
			if (projectConfig.header.displayInfo) {
				setTabs();
			}
		});
	}
	function setTabs() {
		if (!projectConfig.useWebXPanel) document.getElementById('webxpanel-tab').style.display = 'none';
		updateVersionTabHTML();
		updatePageCount();
		setTabsListeners();
		setLogButtonListener();
	}
	function updateVersionTabHTML() {
		serviceModule.loadJSON('./assets/data/version.json', (packages) => {
			if (!packages) return console.log("FILE NOT FOUND");
			Array.from(JSON.parse(packages)).forEach((e) => versionTableBody.appendChild(createTableRow(e)))
		})
	}
	function createTableRow(data) {
		const tableRow = document.createElement('tr');
		for (const value of Object.values(data)) {
			const tableData = document.createElement('td');
			if (value === 'Y') {
				tableData.style.color = "green";
				tableData.innerHTML = '<i class="fas fa-check"></i>&nbsp;&nbsp;Yes';
			}
			else if (value === 'N') {
				tableData.innerHTML = '<i class="fas fa-times"></i>&nbsp;&nbsp;No';
				tableData.style.color = "orange";
			}
			else {
				tableData.textContent = value;
			}
			tableRow.appendChild(tableData);
		}
		return tableRow;
	}
	function updatePageCount() {
		const diagnosticsTableElement = document.getElementById('diagnosticsTableElement');
		const diagnosticPageHeaderElement = document.getElementById('diagnosticPageHeaderElement');
		const listOfPages = projectConfigModule.getNavigationPages();
		const pageCount = document.getElementById('pageCount');

		pageCount.textContent = translateModuleHelper('pagecount', listOfPages.length);
		diagnosticPageHeaderElement.children[2].textContent += ` (${listOfPages.filter(page => page.preloadPage).length})`;
		diagnosticPageHeaderElement.children[3].textContent += ` (${listOfPages.filter(page => page.cachePage).length})`;
		for (const page of listOfPages) {
			const processedPageName = page.navigation.isI18nLabel ? translateModule.translateInstant(page.navigation.label) : page.navigation.label;
			const newTableEntry = createTableRow({ name: processedPageName, count: '', preload: page.preloadPage ? 'Y' : 'N', cached: page.cachePage ? 'Y' : 'N', nodes: '' });
			newTableEntry.setAttribute('id', 'diagnostics-table-' + page.pageName);
			diagnosticsTableElement.appendChild(newTableEntry);
			tableCount[`${page.pageName}`] = {};
		}
	}
	function setTabsListeners() {
		const tabs = ['version-tab', 'webxpanel-tab', 'diagnostics-tab'];
		tabs.forEach((tab) => {
			document.getElementById(tab).addEventListener('click', function () {
				if (this.classList.contains('selected')) return;
				tabs.forEach((tabContent) => tab !== tabContent ? document.getElementById(tabContent + '-content').style.display = "none" : document.getElementById(tabContent + '-content').style.display = "block");
				tabs.forEach((selectedTab) => tab !== selectedTab ? document.getElementById(selectedTab).classList.remove('selected') : "");
				this.classList.add('selected');
			})
		})
	}
	function setLogButtonListener() {
		subscribeLogButton.addEventListener('click', logSubscriptionsCount);
		CrComLib.subscribeState('b', '' + projectConfig.header.diagnostics.logs.receiveStateLogDiagnostics, (value) => logSubscriptionsCount(null, value));
	}
	function logSubscriptionsCount(event, signalValue) {
		const signals = updateSubscriptions();
		const ch5components = {
			ch5ComponentsPageWise: { ...tableCount },
			...componentCount,
			totalCh5ComponentsCurrentlyLoaded: CrComLib.countNumberOfCh5Components(document.getElementsByTagName('body')[0]).total
		}

		const signalNames = document.getElementById('totalSignals').textContent.split(':')[1].trim();
		const subscriptions = document.getElementById('totalSubscribers').textContent.split(':')[1].trim();
		if ((signalValue !== undefined && signalValue === true) || signalValue === undefined) {
			console.log({ signals, ch5components, signalNames, subscriptions });
		}
	}
	function translateModuleHelper(fieldName, fieldValue) {
		return translateModule.translateInstant(`header.info.diagnostics.${fieldName}`) + " " + fieldValue;
	}

	function updateSubscriptions() {
		let tsubscriptions = 0;
		let subscribers = 0;
		let data = [];
		const signals = CrComLib.getSubscriptionsCount();
		for (const [sType, value] of Object.entries(signals)) {
			for (const [signal, details] of Object.entries(value)) {
				tsubscriptions++;
				const signalType = sType != undefined ? sType : "";
				const signalName = signal != undefined ? signal : "";
				const subscriptions = Object.values(details._subscriptions).length - 1;
				data.push({ signalType, signalName, subscriptions });
				subscribers += subscriptions;
			}
		}
		const totalSignals = document.getElementById('totalSignals');
		const totalSubscribers = document.getElementById('totalSubscribers');

		totalSignals.textContent = translateModuleHelper('subscribers', subscribers);
		totalSubscribers.textContent = translateModuleHelper('subscription', tsubscriptions);

		return data;
	}

	/**
	 * private method for page class initialization
	 */
	let loadedImportSnippet = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:template-version-info-import-page', (value) => {
		if (value['loaded']) {
			setTimeout(() => {
				onInit();
			});
			setTimeout(() => {
				CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:template-version-info-import-page', loadedImportSnippet);
				loadedImportSnippet = null;
			});
		}
	});

	/**
	 * All public method and properties are exported here
	 */
	return {
		translateModuleHelper,
		updateSubscriptions,
		tableCount,
		componentCount,
		logSubscriptionsCount
	};
})();
const fireModule = (() => {
    'use strict';

    function onInit() {
        let messageElement = document.getElementById('emergency-message');

        
        function toggleEmergencyMessage() {
            if (messageElement.textContent === 'Fire') {
                messageElement.textContent = 'Incendie';
            } else {
                messageElement.textContent = 'Fire';
            }
        }
 
        setInterval(toggleEmergencyMessage, 2000);
    }

    
    // private method for page class initialization
    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:fire-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:fire-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    }); 

    return {
    };

})();
/**
 * Copyright (C) 2023 to the present, Crestron Electronics, Inc.
 * All rights reserved.
 * No part of this software may be reproduced in any form, machine
 * or natural, without the express written consent of Crestron Electronics.
 * Use of this source code is subject to the terms of the Crestron Software License Agreement 
 * under which you licensed this source code.  
 *
 * This code was automatically generated by Crestron's code generation tool.
*/
/*jslint es6 */
/*global serviceModule, CrComLib */

const page1Module = (() => {
    'use strict';

    // BEGIN::CHANGEAREA - your javascript for page module code goes here         

    /**
     * Initialize Method
     */
    function onInit() {
        //serviceModule.addEmulatorScenarioNoControlSystem("./app/project/components/pages/page1/page1-emulator.json");
        // Uncomment the below line and comment the above to load the emulator all the time.
        // serviceModule.addEmulatorScenario("./app/project/components/pages/page1/page1-emulator.json");    


        function showAlert(message, duration) {
            const alertBox = document.getElementById('customAlert');
            alertBox.innerText = message;
            alertBox.style.display = 'block'; // Show the alert

            // Hide the alert after the specified duration
            setTimeout(() => {
                alertBox.style.display = 'none';
            }, duration);
        }


    }

    /**
     * private method for page class initialization
     */
    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:page1-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    });

    /**
     * All public method and properties are exported here
     */
    return {
    };

    // END::CHANGEAREA

})();