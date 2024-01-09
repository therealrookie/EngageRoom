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