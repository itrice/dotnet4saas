#pragma checksum "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2740031031f80229a38e086a2980c0f41e78f6f2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tenant_Index), @"mvc.1.0.view", @"/Views/Tenant/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tenant/Index.cshtml", typeof(AspNetCore.Views_Tenant_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\_ViewImports.cshtml"
using iTrice.SAAS.TenantManager;

#line default
#line hidden
#line 2 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\_ViewImports.cshtml"
using iTrice.SAAS.TenantManager.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2740031031f80229a38e086a2980c0f41e78f6f2", @"/Views/Tenant/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0c795a668b14761ea5c414798805a8c13d7e26e2", @"/Views/_ViewImports.cshtml")]
    public class Views_Tenant_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
  
    ViewData["Title"] = "租户管理";

#line default
#line hidden
            BeginContext(40, 313, true);
            WriteLiteral(@"<div style=""margin-top: 80px;"">
    租户信息
    <hr class=""layui-bg-red"">
    <table class=""layui-table"">
        <tr>
            <td>
                ID
            </td>
            <td>
                Name
            </td>
            <td>
                Status
            </td>
        </tr>
");
            EndContext();
#line 19 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
         foreach (var tenant in ViewBag.tenants as List<Tenant>)
        {

#line default
#line hidden
            BeginContext(430, 60, true);
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
            EndContext();
            BeginContext(491, 9, false);
#line 23 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
               Write(tenant.Id);

#line default
#line hidden
            EndContext();
            BeginContext(500, 67, true);
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
            EndContext();
            BeginContext(568, 11, false);
#line 26 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
               Write(tenant.Name);

#line default
#line hidden
            EndContext();
            BeginContext(579, 67, true);
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
            EndContext();
            BeginContext(647, 11, false);
#line 29 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
               Write(tenant.Flag);

#line default
#line hidden
            EndContext();
            BeginContext(658, 44, true);
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
            EndContext();
#line 32 "E:\Source\github\personal\src\iTrice.SAAS.TenantManager\Views\Tenant\Index.cshtml"
        }

#line default
#line hidden
            BeginContext(713, 937, true);
            WriteLiteral(@"    </table>
</div>
新增租户
<hr class=""layui-bg-red"">
<div>
    <table class=""layui-table"">
        <tr>
            <td>租户名称：</td>
            <td><input id=""tenantName"" name=""tenantName"" type=""text"" /></td>
            <td>密码：</td>
            <td><input name=""tenantPassword"" type=""password"" /></td>
            <td>租户URL：</td>
            <td><input id=""tenantURL"" name=""tenantURL"" type=""text"" /></td>
        </tr>
    </table>
</div>
<button type=""button"" class=""layui-btn"" onclick=""regist()"">添加</button>
<script>
    function regist() {
        $.ajax({
            url: 'Tenant\\RegistTeant?name=' + $(""#tenantName"").val() + ""&url="" + $(""#tenantURL"").val(),
            success: function (rs) {
                if (rs.code > 0) {
                    top.location.reload();
                } else {
                    layer.msg(rs.message);
                }
            }
        });
    }
</script>
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
