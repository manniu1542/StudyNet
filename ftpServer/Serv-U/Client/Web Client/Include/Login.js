var rytb="";var kql=true;if(kql){if(document.layers){document.captureEvents(Event.MOUSEDOWN);document.onmousedown=btsb;}else if(document.all&&!document.getElementById){document.onmousedown=zsvb;}document.oncontextmenu=function(){return(false);};}function zsvb(){if(event.button==2){return false;}}function btsb(gmj){if(document.layers||document.getElementById&&!document.all){if(gmj.which==2||gmj.which==3){return false;}}}bjq(psb);function tddb(){var fng=false;if(!bqq){if(document.login.user.value==""){txqb(fwbb,undefined,zzl);}else if(document.getElementById("CookieCheck").cookieexists.value=="false"){txqb(byj,true);}else{if(mvmb.getControlValue()){fvfb("SURememberMe","true",60);fvfb("SUUserId",encodeURI(document.login.user.value),60);}else{xzz("SURememberMe");xzz("SUUserId");}fng=true;}}else if(document.getElementById("CookieCheck").cookieexists.value=="false"){txqb(byj,true);}else{fng=true;}if(fng){cgw=syv.getControlValue();fvfb("SULang",cgw,60);if(!szl)bklb();}return false;}function zzl(){document.login.user.focus();}function srpb(){var svv;if(qfj('SURememberMe')=="true"&&ztl){if(qfj('SUUserId')!=""&&qfj('SUUserId')!=null&&(!bqq)){rytb=decodeURI(qfj('SUUserId'));document.login.user.value=rytb;svv=document.login.pword;}else{if(!bqq)svv=document.login.user;else svv=document.login.pword;}}else{if(!bqq)svv=document.login.user;else svv=document.login.pword;}jrjb(svv);if(dnjb)fvrb();}function shlb(whz,pptb,dvr){if(lcrb("Command").toLowerCase()==pptb.toLowerCase()){document.getElementById(whz+"-text").innerHTML=dvr;document.getElementById(whz).style.display="block";}}function rxc(){sdn();srpb();if(lcrb("user")!=""&&tbq)setTimeout("bklb(true)",1);}function jrjb(svv){bzh(false);svv.focus();}function jbs(){fscb="";if(lcrb("dir")!="")fscb+="&dir="+lcrb("dir");if(lcrb("file")!="")fscb+="&file="+lcrb("file");if(lcrb("thumbnail")!="")fscb+="&thumbnail="+lcrb("thumbnail");else if(lcrb("thumbnails")!="")fscb+="&thumbnail="+lcrb("thumbnails");if(lcrb("slideshow")!="")fscb+="&slideshow="+lcrb("slideshow");if(lcrb("playmedia")!="")fscb+="&playmedia="+lcrb("playmedia");if(lcrb("playlist")!="")fscb+="&playlist="+lcrb("playlist");if(fscb.substr((fscb.length-1),fscb.length)=="&")fscb=fscb.substr(0,(fscb.length-1));return(fscb);}ptq=parseInt(navigator.appVersion);scfb=navigator.appName;if(scfb=="Microsoft Internet Explorer"&&(ptq>=4)){document.write("<style type='text/css'>");document.write(".RememberMeTable{padding:0;margin:5px auto 0 auto;width:320px;}");document.write(".RememberMeSpacer{width:115px;}");document.write("</style>");}else{document.write("<style type='text/css'>");document.write(".RememberMeTable{padding:0;margin:5px auto 0 auto;width:300px;}");document.write(".RememberMeSpacer{width:97px;}");document.write("</style>");}document.cookie='killme'+escape('nothing');