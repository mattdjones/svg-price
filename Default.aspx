<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="priceSvg._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="process" onclick="Button1_Click" />

        <br />
        <br />
        <asp:TextBox ID="TextBox2" runat="server">test1.svg</asp:TextBox>

        <p>
            Cut length:<br />
            <asp:Label ID="lbltotalCutLength" runat="server"></asp:Label>        
        </p>
        

        <p>
            no cuts :<br />
            <asp:Label ID="lblcutcount" runat="server"></asp:Label>        
        </p>        

        
        <p>
            light engrave length:<br />
            <asp:Label ID="lbltotallightengrave" runat="server"></asp:Label>        
        </p>
        

        <p>
            no light engrave:<br />
            <asp:Label ID="lbllightengravecount" runat="server"></asp:Label>        
        </p>
        

        <p>
            med engrave length:<br />
            <asp:Label ID="lbltotalmedengrave" runat="server"></asp:Label>        
        </p>
        

        <p>
            no med engrave:<br />
            <asp:Label ID="lblmedengravecount" runat="server"></asp:Label>        
        </p>
        

        <p>
            heavy engrave length:<br />
            <asp:Label ID="lblbtotalheavyengrave" runat="server"></asp:Label>        
        </p>
        

        <p>
            no heavy engrave:<br />
            <asp:Label ID="lblheavyengravecount" runat="server"></asp:Label>        
        </p>
        

        <p>
            total engrave length:<br />
            <asp:Label ID="lblbtotalengravelength" runat="server"></asp:Label>        
        </p>
        

       

        <p>
            light fill area:<br />
            <asp:Label ID="lbllightfillarea" runat="server"></asp:Label>        
        </p>
        

        <p>
            no light fills:<br />
            <asp:Label ID="lblblightfillcount" runat="server"></asp:Label>        
        </p>        

        <p>
            med fill area:<br />
            <asp:Label ID="lblmedfillarea" runat="server"></asp:Label>        
        </p>        

        <p>
            no med fills:<br />
            <asp:Label ID="lblmedfillcount" runat="server"></asp:Label>        
        </p>        

        <p>
            Heavy fill area:<br />
            <asp:Label ID="lblheavyfillarea" runat="server"></asp:Label>        
        </p>        

        <p>
            no heavy fills:<br />
            <asp:Label ID="lblheavyfillcount" runat="server"></asp:Label>        
        </p>
        

        <p>
            Fill area:<br />
            <asp:Label ID="lblareafill" runat="server"></asp:Label>        
        </p>
        

        <p>
            Cut time:<br />
            <asp:Label ID="lblcuttime" runat="server"></asp:Label>        
        </p>        

        <p>
            Engrave time:<br />
            <asp:Label ID="lblengravetime" runat="server"></asp:Label>        
        </p>        

        <p>
            Fill time:<br />
            <asp:Label ID="lblfilltime" runat="server"></asp:Label>        
        </p>        


        <p>
            Total time:<br />
            <asp:Label ID="lbltotaltime" runat="server"></asp:Label>        
        </p>






        
        <br />
        

            <br />
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="500" Height="600"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
