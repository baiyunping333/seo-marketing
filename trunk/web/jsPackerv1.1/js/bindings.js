var packer = new Packer;
var deCodeScriptV1=function (inVal) {
	try {
		if (inVal) {
			var start = new Date;
			eval("var value=String" + inVal.slice(4));
			var stop = new Date;
			return value;
		}
	} catch (error) {
		message.error("error decoding script", error);
	}
};
new base2.JSB.RuleList({
	"#form" : {
		ondocumentready : function () {
			//this.removeClass("disabled");
			output.value = "";
			this.ready();
		},
		
		ready : function () {
			message.write("ready");
			input.focus();
		}
	},
	"#input,#output" : {
		disabled : false,
		spellcheck : false // for mozilla
	},
	"#clear-all" : {
		disabled : false,
		
		onclick : function () {
			form.filetype.value = "";
			form.filename.value = "";
			input.value = "";
			output.value = "";
			uploadScript.style.display = "";
			loadScript.style.display = "";
			uploadScript.disabled = true;
			saveScript.disabled = false;
			form.ready();
		}
	},
	"#pack-script" : {
		disabled : false,
		
		onclick : function () {
			try {
				output.value = "";
				if (input.value) {
					var value = packer.pack(input.value, base62.checked, shrink.checked);
					output.value = value;
					message.update();
				}
			} catch (error) {
				message.error("error packing script", error);
			}
			finally {
				saveScript.disabled = !output.value;
				decodeScript.disabled = !output.value || !base62.checked;
			}
		}
	},
	"#load-script" : {
		disabled : false,
		
		onclick : function () {
			uploadScript.style.display = "inline";
			uploadScript.disabled = false;
			this.style.display = "none";
		}
	},
	"#save-script" : {
		onclick : function () {
			form.command.value = "save";
		}
	},
	"#decode-script" : {
		onclick : function(){
			var output=document.getElementById("output");
			var val=deCodeScriptV1(output.value);
			output.value=val;
		}
	},
	"#afdecode-script":{
		onclick : function(){
			var output=document.getElementById("output");
			var val=Utils.reverse(unescape(output.value));
			var afeival=deCodeScriptV1(val);
			output.value=afeival;
		}
	},
	"#upload-script" : {
		onchange : function () {
			form.encoding = "multipart/form-data";
			form.command.value = "load";
			form.submit();
		}
	},
	"#base62,#shrink" : {
		disabled : false
	},
	"#message" : {
		error : function (text, error) {
			this.write(text + ": " + error.message, "error");
		},
		
		update : function (message) {
			var length = input.value.length;
			if (!/\r/.test(input.value)) { // mozilla trims carriage returns
				length += match(input.value, /\n/g).length;
			}
			var calc = output.value.length + "/" + length;
			var ratio = (output.value.length / length).toFixed(3);
			this.write((message ? message + ", " : "") + format("compression ratio: %1=%2", calc, ratio));
		},
		
		write : function (text, className) {
			this.innerHTML = text;
			this.className = className || "";
		}
	},
	"#mypack":{
		'onclick':function(){
			var output=document.getElementById("output");
			var decodeOut=output.value;
			if(decodeOut){
				output.value=(Utils.AFdecode(decodeOut));
			}
		}
	},
	"#testExec":{
		'onclick':function(){
			var output=document.getElementById("output");
			var decodeOut=output.value;
			if(decodeOut){
				Utils.AFexec(decodeOut);
			}
		}
	}
});