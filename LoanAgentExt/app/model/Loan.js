Ext.define('LoanAgentExt.model.Loan', {
    extend: 'LoanAgentExt.model.Base',

    fields: [
        { name: 'loanId', type: 'string' },
        { name: 'loanOwnerName', type: 'string' },
        { name: 'loanAmount', type: 'number' },
        { name: 'currency', type: 'string' },
        { name: 'startDate', type: 'date' },
        { name: 'endDate', type: 'date' },
        { name: 'loanType', type: 'string' },
        { name: 'loanState', type: 'string' },
        { name: 'createdDateTime', type: 'date' },
        { name: 'createdById', type: 'string' },
        { name: 'updatedDateTime', type: 'date', allowNull: true },
        { name: 'updatedById', type: 'string', allowNull: true },
        { name: 'deletedDateTime', type: 'date', allowNull: true },
        { name: 'deletedById', type: 'string', allowNull: true }
    ]
});