var nkm,nwx,dzj;var mz=false;var hqp=false;var jdpb=false;var vmf=navigator.userAgent.toLowerCase().indexOf('chrome')>-1;if(navigator.userAgent.indexOf("Firefox")!=-1){var bjd=navigator.userAgent.indexOf("Firefox")+8;if(parseInt(navigator.userAgent.charAt(bjd))<=2)mz=true;else if(parseInt(navigator.userAgent.charAt(bjd))>=3)hqp=true;}if(navigator.userAgent.indexOf("Shiretoko")!=-1){hqp=true;}if(navigator.userAgent.toLowerCase().indexOf('safari')!=-1)jdpb=true;function jvc(qgmb){var rzsb="";if(qgmb!=undefined){rzsb=qgmb.replace(eval("/%2F/g"),"/");rzsb=rzsb.replace(eval("/%3A/g"),":");rzsb=rzsb.replace(eval("/%26/g"),"&");rzsb=rzsb.replace(eval("/%24/g"),"$");rzsb=rzsb.replace(eval("/%2C/g"),",");rzsb=rzsb.replace(eval("/%40/g"),"@");rzsb=rzsb.replace(eval("/%23/g"),"#");rzsb=rzsb.replace(eval("/%2B/g"),"+");rzsb=rzsb.replace(eval("/%3D/g"),"=");rzsb=rzsb.replace(eval("/%3B/g"),";");rzsb=decodeURI(rzsb);}return(rzsb);}function mxn(thbb){var yzy=new Date(parseInt(thbb)*1000);return(yzy.toLocaleString());}function vt(nlkb){var xddb={"DLL":1,"ISO":1,"MID":1,"WMV":1,"MPG":1,"SAFARI":1,"SAFURL":1,"FIREFOX":1,"FFURL":1,"MOV":1,"M4A":1,"MDB":1,"MP3":1,"PDF":1,"PPT":1,"PNG":1,"DOC":1,"URL":1,"XLS":1,"ZIP":1,"TXT":1,"HTM":1,"HTML":1,"EXE":1,"VCXPROJ":1};nlkb=nlkb.toUpperCase();switch(nlkb){case"MP4":case"M4P":nlkb="MOV";break;case"RTF":case"RTX":nlkb="DOC";break;case"LOG":nlkb="TXT";break;break;case"POT":case"PPA":case"PPS":case"PPZ":case"PWZ":nlkb="PPT";break;case"WMA":nlkb="MP3";break;case"HTM":case"HTML":if(navigator.userAgent.toLowerCase().indexOf('safari')!=-1)nlkb="SAFARI";else if(navigator.userAgent.indexOf("Firefox")!=-1)nlkb="FIREFOX";break;case"URL":if(navigator.userAgent.toLowerCase().indexOf('safari')!=-1)nlkb="SAFURL";else if(navigator.userAgent.indexOf("Firefox")!=-1)nlkb="FFURL";break;}if(xddb[nlkb])return(nlkb+"Ext");else return("DefaultExt");}function cjfb(nlkb){var xddb={"AVI":1,"BAT":1,"BMP":1,"C":1,"CAB":1,"CHM":1,"CNT":1,"CPP":1,"CRT":1,"CSV":1,"DEF":1,"DLL":1,"DOC":1,"DOT":1,"DSP":1,"DSW":1,"EXE":1,"GIF":1,"H":1,"ANI":1,"HTML":1,"HTM":1,"HXC":1,"HXI":1,"HXK":1,"HXS":1,"HXT":1,"INI":1,"ISO":1,"JPG":1,"JS":1,"MAK":1,"MDB":1,"MDE":1,"MDW":1,"MDZ":1,"MID":1,"MP3":1,"URL":1,"MOV":1,"MPG":1,"MSI":1,"PPT":1,"PDF":1,"POT":1,"PNG":1,"RC":1,"REG":1,"RTF":1,"SCT":1,"SLN":1,"HLP":1,"TTF":1,"TXT":1,"VBS":1,"SAFARI":1,"FIREFOX":1,"VCPROJ":1,"VCXPROJ":1,"VSSSCC":1,"XML":1,"WAV":1,"WMA":1,"WMV":1,"WMZ":1,"WRI":1,"XLS":1,"ZIP":1};nlkb=nlkb.toUpperCase();switch(nlkb){case"MP4":case"M4P":nlkb="MOV";break;case"HTM":case"HTML":case"URL":if(navigator.userAgent.toLowerCase().indexOf('safari')!=-1)nlkb="SAFARI";else if(navigator.userAgent.indexOf("Firefox")!=-1)nlkb="FIREFOX";break;case"POT":case"PPA":case"PPS":case"PPZ":case"PWZ":nlkb="PPT";break;}if(xddb[nlkb])return(nlkb+"Ext");else return("DefaultExt");}function njw(dccb){var pqx=["DRIVE_UNKNOWN","DRIVE_NO_ROOT_DIR","DRIVE_REMOVABLE","DRIVE_FIXED","DRIVE_REMOTE","DRIVE_CDROM","DRIVE_RAMDISK","DRIVE_REMOVABLE_OTHER"];dccb=parseInt(dccb);if((!isNaN(dccb))&&(dccb<pqx.length))return(pqx[dccb]);else return("DRIVE_UNKNOWN");}function mlnb(hhd){mdg=hhd;mdg=mdg.replace(new RegExp("[\(]","g"),"\\(");mdg=mdg.replace(new RegExp("[\)]","g"),"\\)");mdg=mdg.replace(new RegExp("'","g"),"\\'");return(mdg);}function vsgb(){rgtb(tfbb);}function blgb(){if(zlbb){kjk();}}function xpbb(qgmb,rbh,xsh){var ynz=encodeURIComponent(qgmb);if(rbh!=undefined){if(xsh){document.getElementById("a-"+rbh).setAttribute("selected","progress-dir");document.getElementById("dir-info-"+rbh).style.display="none";document.getElementById("a-"+rbh).style.padding="8px 36px 8px 60px";document.getElementById("a-"+rbh).style.margin="-8px 0px -8px -10px";document.getElementById("a-"+rbh).style.cssFloat="none";document.getElementById("a-"+rbh).style.overflow="hidden";document.getElementById("a-"+rbh).style.width="";}else document.getElementById("a-"+rbh).setAttribute("selected","progress");this.setTimeout(function(){location.href='MListDir.htm?Dir='+ynz+'&Session='+jps;if(xsh){document.getElementById("a-"+rbh).setAttribute("selected","done-dir");document.getElementById("dir-info-"+rbh).style.display="block";document.getElementById("a-"+rbh).style.paddingRight="-50px";document.getElementById("a-"+rbh).style.width="70%";document.getElementById("a-"+rbh).style.cssFloat="left";document.getElementById("a-"+rbh).style.backgroundImage='none';}else document.getElementById("a-"+rbh).setAttribute("selected","done");},1500);}else location.href='MListDir.htm?Dir='+ynz+'&Session='+jps;}function pxmb(pyp){vkqb=false;clearTimeout(gyqb);document.getElementById("Image-File").innerHTML="";qycb(pyp);}function xmr(){document.getElementById("GoTo-Layer").style.display='none';document.getElementById("FileInfo").style.display='none';document.getElementById("FileInfo").innerHTML="";document.body.style.backgroundImage='none';document.getElementById("Mobile-Toolbar").style.display='block';document.getElementById("ListMobileFiles").style.display='block';if(whqb){whqb=false;dt=mbnb;setTimeout("srm()",0);}scrollTo(0,djn);}function vph(pqg,slv,wzbb,ptl,whz){var ctbb="";var nwmb="";if(whz!=undefined)nwmb=whz;if(ptl)ctbb="-blue";vsrb='<div id="'+nwmb+'" class="Button-Mobile'+ctbb+'" style="height:33px;width:'+wzbb+'px;" onclick="javascript:'+slv+'">'+pqg+'</div>';return(vsrb);}function wcf(tjcb,vm,htz,vwb,qnh,dqbb,jxv,tvg,jypb){this.FileName=tjcb;this.FileSize=vm;this.FileDate=htz;this.FilePath=vwb;this.FileIsDir=qnh;this.FileIsImage=dqbb;this.FileIsAudio=jxv;this.FileIsVideo=tvg;this.FileIsDrive=jypb;}function ksmb(){location.href=ztqb;}function jzpb(dccb){var pqx=["DRIVE_UNKNOWN","DRIVE_NO_ROOT_DIR","DRIVE_REMOVABLE","DRIVE_FIXED","DRIVE_REMOTE","DRIVE_CDROM","DRIVE_RAMDISK","DRIVE_REMOVABLE_OTHER"];dccb=parseInt(dccb);if((!isNaN(dccb))&&(dccb<pqx.length))return(pqx[dccb]);else return("DRIVE_UNKNOWN");}function zwg(hqkb,hhd){var yhv="";var my=false;if(hhd!=""){if(hqkb)kmbb=vzs;else kmbb=hrf;my=confirm(kmbb);if(my)rhqb(hqkb,hhd);}else alert(gvb);}function xmh(jxp,qm,jsv){qzbb=jsv;document.getElementById("NewPath").value=jsv;document.getElementById("original_path").value=qm;document.getElementById("isdir").value=jxp;document.getElementById("FileInfo").style.display="none";document.getElementById("File-Rename").style.display="block";document.getElementById("NewPath").focus();}function qhcb(){if((nwx!=undefined)&&(nwx.readyState==4)){var tfdb=nwx.responseXML;nwx=undefined;var shcb=kzbb(tfdb,"result");var hqh=qtdb(tfdb,"ResultText");if(shcb!=vzm)alert(hqh);else{lfx();vsgb();}}}function rhqb(jxp,yhv){if(jxp)xxrb="/?Command=DeleteDir";else xxrb="/?Command=Delete";nwx=zyhb("POST",xxrb,qhcb);nwx.setRequestHeader("X-User-Agent",navigator.userAgent);nwx.send("File="+encodeURIComponent(yhv));}function mbd(){if((nkm!=undefined)&&(nkm.readyState==4)){var tfdb=nkm.responseXML;nkm=undefined;var shcb=kzbb(tfdb,"result");var hqh=qtdb(tfdb,"ResultText");var prz;var vbx;var pvfb=document.getElementById("isdir").value;if(pvfb=="false")xh=false;else xh=true;if(shcb!=vzm){if(shcb==tlt){if(xh){kltb=zlwb;fbg="\n\n"+ggcb;}else{kltb=rcv;fbg="\n\n"+hyfb;}alert(jnv(shcb,kltb+qzbb+fbg,hqh));}else alert(hqh);}else{document.getElementById("File-Rename").style.display="none";document.getElementById("NewPath").value="";document.getElementById("NewPath").blur();document.getElementById("original_path").value="";document.getElementById("isdir").value="";lfx();vsgb();}}}function jztb(){var glh=document.getElementById("original_path").value;var sfj=document.getElementById("NewPath").value;var rpn="/?Command=Rename&Dir="+tfbb;nkm=zyhb("POST",rpn,mbd);nkm.setRequestHeader("X-User-Agent",navigator.userAgent);nkm.send("new_path="+encodeURIComponent(sfj)+"&original_path="+encodeURIComponent(glh));return false;}function lfx(){document.getElementById("ListMobileFiles").innerHTML="";document.body.style.backgroundImage='url(/Common/Images/BusyGrid.gif)';document.body.style.backgroundRepeat='no-repeat';document.body.style.backgroundFixed='fixed';document.body.style.backgroundPosition='50% 50%';document.getElementById("GoTo-Layer").style.display="none";document.getElementById("FileInfo").style.display='none';document.getElementById("FileInfo").innerHTML="";document.getElementById("Mobile-Toolbar").style.display='block';document.getElementById("ListMobileFiles").style.display='block';}function lzp(){document.getElementById("NewPath").value="";document.getElementById("original_path").value="";document.getElementById("isdir").value="";document.getElementById("File-Rename").style.display="none";document.getElementById("FileInfo").style.display="block";}function jtdb(){var gjw=0;sxb=decodeURI(tfbb);gjw=sxb.lastIndexOf("/");if(gjw<0)gjw=sxb.lastIndexOf("%2F");if(gjw<0)tfbb=cwp;else{if(gjw==0)tfbb="/";else tfbb=sxb.substring(0,gjw);}location.href='MListDir.htm?Dir='+tfbb+'&Session='+jps;}function clv(){document.getElementById("GoToPath").value=decodeURIComponent(tfbb);document.getElementById("GoTo-Layer").style.display="block";document.body.style.backgroundImage='url(/Web%20Client/Mobile/Images/MobileInfoBG.gif)';document.body.style.backgroundRepeat='repeat-x';document.body.style.backgroundFixed='fixed';document.getElementById("Mobile-Toolbar").style.display='none';document.getElementById("ListMobileFiles").style.display='none';document.getElementById("GoToPath").focus();}function GoTo(){var tsf=document.getElementById("GoToPath").value;lfx();xpbb(tsf);return false;}function gxgb(){xmr();}function zyhb(mrrb,mtlb,jzy){var mhd=null;try{mhd=new ActiveXObject("Msxml2.XMLHTTP");}catch(gmj){try{mhd=new ActiveXObject("Microsoft.XMLHTTP");}catch(gmj){try{mhd=new XMLHttpRequest();}catch(gmj){alert(zkjb);}}}if(mhd){if((mrrb)&&(mtlb)){mhd.open(mrrb,mtlb+wpg(),true);if(mrrb.toUpperCase()=="POST")mhd.setRequestHeader("Content-Type","application/x-www-form-urlencoded");}if(jzy)mhd.onreadystatechange=jzy;}return(mhd);}function rwy(){if((dzj!=undefined)&&(dzj.readyState==4)){var vch=55000;if(xm)vch=10000;var tfdb=dzj.responseXML;dzj=undefined;var shcb=kzbb(tfdb,"result");var hqh=qtdb(tfdb,"ResultText");if(shcb!=vzm){alert(jnv(0,"",hqh));}else setTimeout("scwb()",vch);}}function scwb(){var ryvb="/?Command=NOOP";dzj=zyhb("POST",ryvb,rwy);dzj.setRequestHeader("X-User-Agent",navigator.userAgent);dzj.send(null);}