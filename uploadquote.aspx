<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uploadquote.aspx.cs" Inherits="priceSvg.uploadquote" EnableEventValidation="false" %>
<%@ Register TagPrefix="zapp" TagName="svgQuote" Src="~/userControl/svgDesignQuote.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="styles/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    
    <link href="uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="uploadify/swfobject.js" type="text/javascript"></script>
    <script src="uploadify/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="uploadify/jquery.uploadify-3.1___.min.js" type="text/javascript"></script>

    <script src="Scripts/jquery-ui-1.8.18.custom.min.js" type="text/javascript"></script>
    

    <link href="styles/default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">       
       $(document).ready(function() {
          // $("#zappquote_dialog").dialog({
          //          autoOpen: false
          //      });            
        });
	</script>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>


        <zapp:svgQuote runat="server" ID="zappquote" />
    
    </div>
    </form>
</body>
</html>
