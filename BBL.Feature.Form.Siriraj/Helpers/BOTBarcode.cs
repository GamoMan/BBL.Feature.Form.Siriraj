using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBL.Feature.Form.Siriraj.Helpers
{
    public class BOTBarcode
    {
        const string _Prefix = "|";
        const string _CR = "\r\n";

        //
        // BOT Barcode generator
        //
        public struct barcode
        {
            public string Prefix;
            public string TaxID;
            public string Reference1;
            public string Reference2;
            public string Amount;

            public barcode(string vprefix,string vtaxid, string vsuffix, string vref1, string vref2, string vamount)
            {
                Prefix = vprefix;
                TaxID = vtaxid+ vsuffix + _CR;
                Reference1 = vref1+ _CR;
                Reference2 = vref2+ _CR;
                Amount = vamount;
            }
        }
        public static string genBarcode(string taxid, string suffix,string ref1, string ref2, float amount)
        {            
            string ramount = amount.ToString("0.00").Replace(".", "");
            barcode m = new barcode(_Prefix, taxid,suffix,ref1,ref2,ramount);            
            string code = m.Prefix+m.TaxID+m.Reference1+m.Reference2+ m.Amount;
            return code;
        }

        //
        // BOT QRCode generator
        //
        public struct qrbarcode
        {
            public string Prefix;
            public string TaxID;
            public string Suffix;
            public string Reference1;
            public string Reference2;
            public string TotalAmount;
            public string TransctionType;
            public string DueDate;
            public string Quantity;
            public string SaleAmount;
            public string VatRate;
            public string VatAmount;
            public string SellerVatBranchID;
            public string BuyerTaxID;
            public string BuyerVatBranchID;
            public string BuyerName;
            public string Reference3;
            public string ProxyID;
            public string ProxyType;
            public string NetAmount;
            public string IncomeType;
            public string HoldingTaxRate;
            public string HoldingTaxAmount;
            public string HoldingTaxCondition;

            public qrbarcode(string vTaxID,string vSuffix, string vReference1, string vReference2, float vTotalAmount,string vTransctionType, string vDueDate,
                int vQuantity,float vSaleAmount,float vVatRate,float vVatAmount, string vSellerVatBranchID, string vBuyerTaxID,string vBuyerVatBranchID, string vBuyerName,
                string vReference3, string vProxyID,string vProxyType,float vNetAmount,string vIncomeType,float vHoldingTaxRate, float vHoldingTaxAmount,string vHoldingTaxCondition)
            {
                Prefix = _Prefix;
                TaxID = vTaxID;
                Suffix = vSuffix + _CR;
                Reference1 = vReference1 + _CR;
                Reference2 = vReference2 + _CR;
                TotalAmount = vTotalAmount.ToString("0.00").Replace(".", "") + _CR;
                TransctionType = vTransctionType + _CR;
                DueDate = vDueDate + _CR;
                Quantity = vQuantity.ToString("0") + _CR;
                SaleAmount = vSaleAmount.ToString("0.00").Replace(".", "") + _CR;
                VatRate = vVatRate.ToString("0.00").Replace(".", "") + _CR;
                VatAmount = vVatAmount.ToString("0.00").Replace(".", "") + _CR;
                SellerVatBranchID = vSellerVatBranchID + _CR;
                BuyerTaxID = vBuyerTaxID + _CR;
                BuyerVatBranchID = vBuyerVatBranchID + _CR;
                BuyerName = vBuyerName + _CR;
                Reference3 = vReference3 + _CR;
                ProxyID = vProxyID + _CR;
                ProxyType = vProxyType + _CR;
                NetAmount = vNetAmount.ToString("0.00").Replace(".", "") + _CR;
                IncomeType = vIncomeType + _CR;
                HoldingTaxRate = vHoldingTaxRate.ToString("0.00").Replace(".", "") + _CR;
                HoldingTaxAmount = vHoldingTaxAmount.ToString("0.00").Replace(".", "") + _CR;
                HoldingTaxCondition = vHoldingTaxCondition;
            }
        }
        public static string genQRcode(string taxid, string suffix, string ref1, string ref2, float amount)
        {
            //string ramount = amount.ToString("0.00").Replace(".", "");
            qrbarcode m = new qrbarcode(taxid, suffix, ref1,ref2,amount,"","",0,0,0,0,"","","","","","","",0,"",0,0,"");
            string code = m.Prefix + m.TaxID + m.Reference1 + m.Reference2 + m.TotalAmount+m.TransctionType+m.DueDate+m.Quantity+m.SaleAmount+m.VatRate+m.VatAmount+
                          m.SellerVatBranchID+m.BuyerTaxID+m.BuyerVatBranchID+m.BuyerName+m.Reference3+m.ProxyID+m.ProxyType+m.NetAmount+m.IncomeType+m.HoldingTaxRate+
                          m.HoldingTaxAmount+m.HoldingTaxCondition;
            return code;
        }
    }
}