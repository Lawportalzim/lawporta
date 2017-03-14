<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Kay.Site.Common.CustomControls.Menu" %>
<!-- left menu starts -->
<div class="span2 main-menu-span">
	<div class="well nav-collapse sidebar-nav">
		<ul class="nav nav-tabs nav-stacked main-menu">
			<li class="nav-header hidden-tablet">Main</li>
			<li><a class="ajax-link" href="/admin/dashboard/"><i class="icon-home"></i><span class="hidden-tablet"> Dashboard</span></a></li>			
			<li><a class="ajax-link" href="/admin/cases/"><i class="icon-edit"></i><span class="hidden-tablet"> Cases</span></a></li>
            <li><a class="ajax-link" href="/admin/categories/"><i class="icon-tasks"></i><span class="hidden-tablet"> Categories</span></a></li>	
            <li><a class="ajax-link" href="/admin/recyclebin/"><i class="icon-trash"></i><span class="hidden-tablet"> Recycle Bin</span></a></li>
            <li><a class="ajax-link" href="/admin/help/edit.aspx?id=1"><i class="icon-help"></i><span class="hidden-tablet"> Help</span></a></li>					
			<li class="nav-header hidden-tablet">Account Management</li>
            <li><a class="ajax-link" href="/admin/companies/"><i class="icon-briefcase"></i><span class="hidden-tablet"> Companies</span></a></li>
			<li><a href="/admin/logout.aspx"><i class="icon-lock"></i><span class="hidden-tablet"> Logout</span></a></li>
		</ul>		
	</div><!--/.well -->
</div><!--/span-->
<!-- left menu ends -->
<script>
    $('document').ready(function () {
       $() 
    });
  </script>