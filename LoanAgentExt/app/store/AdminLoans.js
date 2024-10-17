Ext.define('LoanAgentExt.store.AdminLoans', {
    extend: 'Ext.data.Store',

    alias: 'store.adminloans',

    model: 'LoanAgentExt.model.Loan',

    storeId: 'adminloans',
    autoLoad: false
});