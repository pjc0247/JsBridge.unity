mergeInto(LibraryManager.library, {
	JsReturn: function(pid, message) {
		JsBridge.callbacks[pid](JSON.parse(message));
		delete JsBridge.callbacks[pid];
	}
});
var JsBridge = {
	callbacks: {},

	pid: 0,
	Call: function(name, message, callback) {
		gameInstance.SendMessage("JsBridge", "OnMessage", {
			pid: this.pid ++,
			message: JSON.stringify(message)
		});
	}
};
