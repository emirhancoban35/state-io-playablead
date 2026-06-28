mergeInto(LibraryManager.library, {

    CallStoreRedirect: function(url) {
        var storeUrl = UTF8ToString(url);
        console.log("[MRAID] Store URL: " + storeUrl);
        
        if (typeof mraid !== 'undefined') {
            mraid.open(storeUrl);
        } 
        else if (typeof window !== 'undefined') {
            window.open(storeUrl);
        }
    },

    LogPlayableEvent: function(eventName) {
        var eventStr = UTF8ToString(eventName);
        
        if (typeof dapi !== 'undefined' && dapi.isReady()) {
            // dapi.logEvent(eventStr);
        }
        
        if (typeof mraid !== 'undefined' && typeof mraid.logEvent === 'function') {
            // mraid.logEvent(eventStr); 
        }
    }
});