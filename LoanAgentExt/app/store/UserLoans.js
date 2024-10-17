Ext.define('LoanAgentExt.store.UserLoans', {
    extend: 'Ext.data.Store',

    alias: 'store.userloans',

    model: 'LoanAgentExt.model.Loan',

    storeId: 'userloans',
    autoLoad: false
});
