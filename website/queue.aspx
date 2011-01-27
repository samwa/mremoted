<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="queue.aspx.cs" Inherits="website.queue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="scripts/jquery1.4.js"></script> 
	<script type="text/javascript" src="scripts/amq.jquery.adapter.js"></script> 
 
	<script type="text/javascript" src="scripts/amq.js"></script> 
	<script type="text/javascript" src="scripts/chat.js"></script> 
    <script type="text/javascript">

    // Note, normally you wouldn't just add an onload function in this
    // manner. In fact, you typically want to fire this method on the
    // document.onready event, however this type of functionality is verbose
    // and best left to the domain of your favorite js library.
    //
    // For example, in jQuery the following onload would be replaced with:
    // jQuery(function() {
    //     org.activemq.Amq.init({ uri: 'amq' });
    //     org.activemq.Chat.init();
    // }
    window.onload = function () {
        org.activemq.Amq.init({ uri: 'amq', logging: true, timeout: 45, clientId: (new Date()).getTime().toString() });
        org.activemq.Chat.init();
    };
	</script>    

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
