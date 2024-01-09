
const selectlocalityModule = (() => {
    'use strict';

    function onInit() {
        const page1 = document.getElementById("selectlocality-page");
        const localUseButton = page1.querySelector('#' + "localUseColumn");
        const meetingButton = page1.querySelector('#' + "meetingColumn");
        const homeButton = page1.querySelector('#' + "navButton");

        localUseButton.addEventListener('click', () => {
            CrComLib.publishEvent('n', 'controlPages.page', 3);
        });

        meetingButton.addEventListener('click', () => {
            CrComLib.publishEvent('n', 'controlPages.page', 7);
        });

        homeButton.addEventListener('click', () => {
            CrComLib.publishEvent('n', 'controlPages.page', 1);
        });
    }


    let loadedSubId = CrComLib.subscribeState('o', 'ch5-import-htmlsnippet:selectlocality-import-page', (value) => {
        if (value['loaded']) {
            onInit();
            setTimeout(() => {
                CrComLib.unsubscribeState('o', 'ch5-import-htmlsnippet:selectlocality-import-page', loadedSubId);
                loadedSubId = '';
            });
        }
    });

    return {
    };


})();