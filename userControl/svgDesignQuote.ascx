<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="svgDesignQuote.ascx.cs" Inherits="priceSvg.userControl.svgDesignQuote" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


    
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {

            //uploadify v3...
//            $('#file_upload').uploadify({
//                'swf': 'uploadify/uploadify.swf',
//                'uploader': 'uploadify/uploader.ashx',
//                'onSelect': function (file) {
//                    $('#zappquote_divQuote').html('');
//                },
//                'cancelImg': 'uploadify/cancel.png',
//                'multi': false,
//                'fileDesc': 'SVG Image Files',
//                'fileExt': '*.svg',
//                'queueSizeLimit': 1,
//                'sizeLimit': 10485760,
//                'folder': 'imgLibrary',
//                'buttonText': 'Select .SVG File',
//                'onComplete': function (event, queueID, fileObj, response, data) {                    
//                    $('#zappquote_fileName').html(fileObj.name);
//                    $('#cFileName').val(fileObj.name);
//                    $('#zappquote_imgClearForm').toggleClass('imgHide');
//                    checkForm();
//                    //                    $('#zappquote_btnCalc').attr('disabled', false);
//                }


                $('#file_upload').uploadify({
                'uploader': 'uploadify/uploadify.swf',
                'script': 'uploadify/uploader.ashx',
                'cancelImg': 'uploadify/cancel.png',
                'multi': false,
                'fileDesc': 'SVG Image Files',
                'fileExt': '*.svg',
                'queueSizeLimit': 1,
                'sizeLimit': 1048576000000,
                'folder': 'imgLibrary',
                'buttonText': 'Select .SVG File',
                'onComplete': function (event, queueID, fileObj, response, data) {
                    $('#zappquote_fileName').html(fileObj.name);
                    $('#cFileName').val(fileObj.name);
                    $('#zappquote_imgClearForm').toggleClass('imgHide');
                    checkForm();
                    //                    $('#zappquote_btnCalc').attr('disabled', false);
                },
                'auto': true
            });

            //});
        });

        function cQuantity(dir) {

            if (dir == "add") {
                document.getElementById("zappquote_svgQuantity").value++;
            }

            if (dir == "sub") {
                if (document.getElementById("zappquote_svgQuantity").value == "1") {
                    alert("There must be a quantity of at least 1.");
                }
                else {
                    document.getElementById("zappquote_svgQuantity").value--;
                }
            }

        }

        function checkQuantity() {
            
            var cVal = document.getElementById("zappquote_svgQuantity").value;

            if (cVal == "0" || cVal == "") {
                alert("There must be a quantity of at least 1.");
                document.getElementById("zappquote_svgQuantity").value = 1;
            }
            else {
                var anum = /(^\d+$)/;
                if (!anum.test(cVal)) {
                    alert("Quantity value must be a number.");
                    document.getElementById("zappquote_svgQuantity").focus();
                    return false;
                }
            }
        }

        function checkForm() {
            var proceed = false;
            if ( ($('#cFileName').val() != "") && ($('#zappquote_svgMaterial').val() != "") && ($('#zappquote_svgMm').val() != "") && ($('#zappquote_materialColour').val() != "") ) {
                proceed = true;
            }
            else {
                proceed = false;
            }     
            if (proceed) {
                $('#zappquote_btnCalc').attr('disabled', false);
            }
            else {
                $('#zappquote_btnCalc').attr('disabled', true);
            }
        }

        

        function clearForm() {
            var cf = $('#cFileName').val();   
            if (confirm("Are you sure you want to delete this file?")) {

                if (($('#cFileName').val() != "")) {
                    $.post("clearForm.ashx", { cFile: cf },
                       function (data) {
                           //alert(data);
                    });
                }
                $('#zappquote_fileName').html('');
                $('#zappquote_imgClearForm').toggleClass('imgHide');
                // Use a whitelist of fields to minimize unintended side effects.
                $(':text, :password, :file, SELECT', '#form1').val('');
                // De-select any checkboxes, radios and drop-down menus
                $(':input', '#form1').removeAttr('checked').removeAttr('selected');

            }
        }
            
        // ]]>
        </script>
 
 <asp:ScriptManager ID="asm" runat="server" />

<div id="centre">

    <img src="img/laser_title.gif" alt="Laser Cutting Title" />

    <p class="pageIntro">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam id imperdiet mauris. Nam a quam id metus dictum iaculis. Vestibulum aliquet odio vitae erat volutpat at dignissim justo venenatis.
    </p>

    <p class="pageIntro">
        Vivamus lobortis erat nunc, ut dignissim mauris. Nunc tincidunt justo at eros fringilla placerat. 
        <a href="#" >Click here and learn how to submit your own, cut ready files!</a>
    </p>

    <div class="boxContainer">
        <div class="boxTop">
    
        </div>

        <div class="boxMiddle">
            <asp:Label CssClass="formSmallWhite" ID="lbluploaddesc" runat="server">10mb max .svg format. <a href="#" class="altLink" >Learn more..</a></asp:Label>

            <div class="boxMargin">
                <input id="file_upload" type="file" name="file_upload" />
                <input type="hidden" id="cFileName" name="cFileName" />

                <div id="fileName" runat="server" style="float: left;" class="formMedWhite"></div>
                &nbsp; 
                <asp:Image ID="imgClearForm" runat="server" CssClass="cursorHand imgHide" ImageUrl="~/uploadify/cancel.png" ToolTip="Clear form and delete file"  />
                <span style="clear: both;"></span>
                <br />
                <asp:Label CssClass="formMedWhite" ID="lblSsvgFileName" runat="server" ></asp:Label>
                <br />
                <asp:Label CssClass="formMedWhite" ID="lblSvgTemplateSize" runat="server" ></asp:Label>
            </div>            
        </div>        
        <div class="boxBottom">
        
        </div>
    </div>

    <div id="dialog" class="errorBox" runat="server" >
        
    </div>

    <table cellpadding="5" width="100%">
    
    <tr>
        <td colspan="2"><hr /></td>
    </tr>

    <tr>
        <td class="formBold" valign="top">Quantity</td>
        <td valign="top">
            <a href="javascript:cQuantity('sub');"><img src="img/minus.png" border="0" width="20" height="20" style="margin-bottom:-5px;" /></a>
            <asp:TextBox ID="svgQuantity" runat="server" Width="20" Text="1"></asp:TextBox>
            <a href="javascript:cQuantity('add');"><img src="img/plus.png" border="0" width="20" height="20" style="margin-bottom:-5px;" /></a>        
        </td>
    </tr>

    <tr>
        <td colspan="2"><hr /></td>
    </tr>

    <tr>
        <td class="formBold" valign="top">Choose a material</td>
        <td valign="top"> 
                    
            <asp:DropDownList runat="server" 
            SelectionMode="Single" ID="svgMaterial" 
            Width="270" 
            AutoPostBack="false" 
            DataTextField="materialName" 
            DataValueField="materialTypeid" 
            AppendDataBoundItems="true"  EnableViewState="true" CssClass="blueList" >                           
            </asp:DropDownList>
            
            <asp:CascadingDropDown ID="ccdSvgMaterial" 
                ServicePath="~/srvc/ccDropDowns.asmx" 
                ServiceMethod="getMaterials" 
                Category="material" 
                TargetControlID="svgMaterial" 
                PromptText="Select a Material" 
                runat="server">
            </asp:CascadingDropDown>

            <br />
            <br />
            <asp:Label runat="server" id="lblMaterialInfo">View our full material catalogue <a href="#">here</a>...</asp:Label>
            <br />
                                    
        </td>
    </tr>

    <tr>
        <td colspan="2"><hr /></td>
    </tr>

    <tr>
        <td class="formBold" valign="top"></td>
        <td valign="top">        
            <asp:DropDownList id="svgMm" Width="270" runat="server" 
                DataSourceId="materialMMid" AutoPostBack="false" DataTextField="materialMm" 
                DataValueField="materialMmId" AppendDataBoundItems="true" CssClass="blueList">                           
            </asp:DropDownList> 

            <asp:CascadingDropDown ID="ccMm" 
                ServicePath="~/srvc/ccDropDowns.asmx" 
                ServiceMethod="getMm" 
                Category="mm" 
                TargetControlID="svgMm" 
                PromptText="Material thickness" 
                ParentControlID="svgMaterial"
                runat="server">
            </asp:CascadingDropDown>
        
            <br />
            <asp:RequiredFieldValidator ID="valSvgMm" runat="server" ForeColor="Red" ControlToValidate="svgMm" Text="Material Thickness" ></asp:RequiredFieldValidator>

            <asp:SqlDataSource ID="materialMMid" runat="server" 
            ConnectionString="<%$ ConnectionStrings:zapCartDb %>" 
            SelectCommand="SELECT * FROM [tbl_materialMm] WHERE activestatus = 1 ORDER BY materialMm">  
            
            </asp:SqlDataSource>  
              
        </td>
    </tr>

    <tr>
        <td class="formBold" valign="top"></td>
        <td valign="top">        
            <asp:DropDownList id="materialColour" Width="270" runat="server" DataSourceId="colourId" CssClass="blueList" AutoPostBack="false" DataTextField="colourName" DataValueField="colourId" AppendDataBoundItems="true">
                <asp:ListItem Text="Material colour"></asp:ListItem>            
            </asp:DropDownList> 
        
            <br />
            <asp:RequiredFieldValidator ID="valCol" runat="server" ForeColor="Red" ControlToValidate="materialColour" Text="Please select a colour." ></asp:RequiredFieldValidator>

            <asp:SqlDataSource ID="colourId" runat="server" 
            ConnectionString="<%$ ConnectionStrings:zapCartDb %>" 
            SelectCommand="SELECT * FROM [tbl_zapColours] WHERE activestatus = 1 ORDER BY colourName">  
            
            </asp:SqlDataSource>  
              
        </td>
    </tr>

    <tr>
        <td colspan="2"><hr /></td>
    </tr>

    <tr>
        <td class="formBold" valign="top">Price your job</td>
        <td valign="top">        
            <asp:Button ID="btnCalc" runat="server" Text="Calculate" 
                onclick="btnCalc_Click" Enabled="false" CssClass="blackbutton" /> 
        
            <br />

            <div id="divQuote" runat="server">            
            </div>            
                
            <br />
        </td>
    </tr>


    </table>

    <asp:Literal ID="errTxt" runat="server"></asp:Literal>
    
</div>


	


<hr />
<b>Debug info below:</b>



<asp:Literal ID="litErr" runat="server"></asp:Literal>

