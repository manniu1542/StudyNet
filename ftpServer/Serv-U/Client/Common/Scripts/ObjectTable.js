AW.XML.SUObjectTable=AW.XML.Table.subclass();AW.XML.SUObjectTable.create=function(){var jd=this.prototype;var ztrb=this.superclass.prototype;jd.init=function(gmt,skw,grx){ztrb.init.call(this);this.setURL(jydb+gmt+wpg()+((skw>0)?"&ID="+skw:""));this.initTable(grx);};jd.initTable=function(grx){this.setRequestHeader("User-Agent","Serv-U");if(grx)this.setColumns(grx);else this.setColumns(["@name","@val","@inherited","@type"]);};jd.GetCollectionByAttrib=function(tdnb,phkb,vjk){var fmjb="//"+tdnb+"[@"+phkb+"='"+vjk+"']";return(this.GetCollection(fmjb));};jd.GetCollection=function(fmjb){var srnb;var tfdb=this.getXML();if(tfdb){if(AW.ie)srnb=tfdb.selectNodes(fmjb);else srnb=this.SelectNodesFF(tfdb,fmjb);}return(srnb);};jd.SelectNodesFF=function(zxt,fmjb){var srnb=new Array();var tfdb=this.getXML();var fdj=tfdb.createNSResolver(tfdb.documentElement);var ftw=tfdb.evaluate(fmjb,zxt,fdj,XPathResult.ORDERED_NODE_SNAPSHOT_TYPE,null);for(var ghg=0;ghg<ftw.snapshotLength;ghg++)srnb[ghg]=ftw.snapshotItem(ghg);return(srnb);};jd.GetNodeValue=function(zxt,phkb){var fmjb="./@"+phkb,vjk="";if(AW.ie){var xqnb=zxt.selectSingleNode(fmjb);if(xqnb)vjk=xqnb.text;else vjk=undefined;}else{var srnb=this.SelectNodesFF(zxt,fmjb);if(srnb.length)vjk=srnb[0].textContent;else vjk=undefined;}return(vjk);};jd.FillForm=function(Form,tkx,xwnb,hgy){if(Form!=undefined){if(hgy!=true)bzh(true);Form.reset();this.setAsync(true);this.request();jd.fnResponse=this.response;this.response=function(pqg){this.fnResponse(pqg);for(var bynb=0;(bynb<Form.length);bynb++){var Element=Form.elements[bynb];if(Element!=undefined){var srnb=this.GetCollectionByAttrib("var","name",this.FormatElementName(Element.name,true));if((srnb!=undefined)&&(srnb.length>0)){var vjk=this.GetNodeValue(srnb[0],"val");var mqgb=this.GetNodeValue(srnb[0],"defaultval");var rjj=this.GetNodeValue(srnb[0],"default");var slp=this.GetNodeValue(srnb[0],"type");if(vjk!=undefined){if(mqgb!=undefined)Element.defaultValue=hdmb(mqgb);if((slp=="3")&&(rjj=="1"))Element.value="";else Element.value=hdmb(vjk);if(Element.onchange!=undefined)Element.onchange();}}}}if(tkx!=undefined)dwx(vvp,tkx,this.m_fnDialogDismissal);if(xwnb!=undefined){setTimeout(function(){xwnb.getContent("box/text").element().focus();});}if(this.m_fnOnFillForm!=undefined)this.m_fnOnFillForm(this.m_OnFillFormObj,this);this.response=this.fnResponse;};}};jd.SubmitForm=function(Form,tkx,hclb,wkrb,mfgb){if(Form!=undefined){var xch=((tkx!=undefined)&&(tkx.length!=0));if(xch||wkrb)hyqb(true);this.setRequestMethod("POST");for(var bynb=0;(bynb<Form.length);bynb++){var Element=Form.elements[bynb];if(Element!=undefined){this.setParameter(this.FormatElementName(Element.name),Element.value);}}this.request();if(xch||wkrb){this.response=function(tfdb){var shcb=parseInt(tfdb.getElementsByTagName("result")[0].childNodes[0].nodeValue);var hqh=tfdb.getElementsByTagName("ResultText")[0].childNodes[0].nodeValue;hyqb(false);if(shcb!=vzm){txqb(jnv(0,"",hqh));if(mfgb!=undefined){var qvq="";if(tfdb.getElementsByTagName("ObjectID")[0].childNodes[0]!=undefined)qvq=parseInt(tfdb.getElementsByTagName("ObjectID")[0].childNodes[0].nodeValue);mfgb(qvq);}}else{if(tkx!=undefined)trc(tkx);if(hclb!=undefined){var qvq=parseInt(tfdb.getElementsByTagName("ObjectID")[0].childNodes[0].nodeValue);hclb(qvq);}}};}}};jd.FormatElementName=function(phkb,kfwb){var ttbb=phkb;if(kfwb){if((ttbb!=undefined)&&(ttbb.lastIndexOf('_')>=0))ttbb=phkb.substr(phkb.lastIndexOf('_')+1);}else{if((ttbb!=undefined)&&(ttbb.indexOf('_')>=0))ttbb=phkb.substr(phkb.indexOf('_')+1);}return(ttbb);};};