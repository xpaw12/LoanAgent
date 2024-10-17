Ext.define('LoanAgentExt.view.main.LoansGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'loansgrid',

    title: 'My Loans',

    store: {
        type: 'userloans'
    },

    columns: [
        { text: 'Loan Amount', dataIndex: 'loanAmount', flex: 1 },
        { text: 'Currency', dataIndex: 'currency', flex: 1 },
        { text: 'Loan Type', dataIndex: 'loanType', flex: 1 },
        { text: 'Loan State', dataIndex: 'loanState', flex: 1 },
        { text: 'Start Date', dataIndex: 'startDate', flex: 1 },
        { text: 'End Date', dataIndex: 'endDate', flex: 1 },
        { text: 'Created Date', dataIndex: 'createdDateTime', flex: 1 },
        {
            xtype: 'actioncolumn',
            text: 'Actions',
            width: 100,
            items: [{
                iconCls: 'fa fa-arrow-right custom-action-icon',
                tooltip: 'Submit Loan',
                handler: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanId = record.get('loanId');

                    Ext.Msg.confirm('Confirm Submission', 'Are you sure you want to submit this loan?', function (btn) {
                        if (btn === 'yes') {
                            grid.up('loansgrid').submitLoan(loanId);
                        }
                    });
                },
                isActionDisabled: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanState = record.get('loanState');

                    return loanState !== 'InEditMode';
                },
                scope: this
            }, {
                iconCls: 'fa fa-pen custom-action-icon',
                tooltip: 'Edit Loan',
                handler: function (grid, rowIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    grid.up('loansgrid').showEditLoanForm(record, grid);
                },
                isActionDisabled: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanState = record.get('loanState');

                    return loanState !== 'InEditMode';
                },
            }, {
                iconCls: 'fa fa-trash custom-action-icon',
                tooltip: 'Delete Loan',
                handler: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanId = record.get('loanId');

                    Ext.Msg.confirm('Confirm Deletion', 'Are you sure you want to delete this loan?', function (btn) {
                        if (btn === 'yes') {
                            grid.up('loansgrid').deleteLoan(loanId);
                        }
                    });
                },
                isActionDisabled: function (view, rowIndex, colIndex, item, record) {
                    return record.get('loanState') === 'Approved';
                }
            }]
        }
    ],

    listeners: {
        itemclick: function (view, record) {
        }
    },

    submitLoan: function (loanId) {
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/change-status',
            method: 'PUT',
            jsonData: {
                loanId: loanId,
                newLoanState: 'Submitted'
            },
            success: function (response) {
                Ext.Msg.alert('Success', 'Loan submitted successfully!', function () {
                    window.location.reload();
                },);
            },
            failure: function () {
                Ext.Msg.alert('Error', 'Failed to update loan status.');
            },
            scope: this
        });
    },
    showEditLoanForm: function (record, grid) {
        const form = Ext.create('Ext.form.Panel', {
            bodyPadding: 10,
            defaultType: 'textfield',
            items: [
                { name: 'loanId', fieldLabel: 'Loan ID', value: record.get('loanId'), readOnly: true, hidden: true },
                { name: 'loanAmount', fieldLabel: 'Loan Amount', value: record.get('loanAmount'), allowBlank: false },
                {
                    xtype: 'combo',
                    fieldLabel: 'Currency',
                    name: 'currency',
                    store: ['Gel', 'Usd', 'Eur'],
                    value: record.get('currency'),
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'combo',
                    fieldLabel: 'Loan Type',
                    name: 'loanType',
                    store: ['QuickLoan', 'CarLoan', 'Installment'],
                    value: record.get('loanType'),
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Start Date',
                    name: 'startDate',
                    value: record.get('startDate'),
                    format: 'Y-m-d',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'End Date',
                    name: 'endDate',
                    value: record.get('endDate'),
                    format: 'Y-m-d',
                    allowBlank: false
                }
            ],
            buttons: [
                {
                    text: 'Save',
                    handler: function () {
                        const formValues = form.getValues();
                        grid.up('loansgrid').editLoan(formValues);

                        editWindow.close();
                    }
                },
                {
                    text: 'Cancel',
                    handler: function () {
                        editWindow.close();
                    }
                }
            ]
        });

        const editWindow = Ext.create('Ext.window.Window', {
            title: 'Edit Loan',
            modal: true,
            layout: 'fit',
            items: [form]
        });

        editWindow.show();
    },
    editLoan: function (formValues) {
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/edit',
            method: 'PUT',
            jsonData: {
                loanId: formValues.loanId,
                loanAmount: formValues.loanAmount,
                currency: formValues.currency,
                startDate: formValues.startDate,
                endDate: formValues.endDate,
                loanType: formValues.loanType
            },
            success: function (response) {
                Ext.Msg.alert('Success', 'Loan updated successfully!', function () {
                    window.location.reload();
                });
            },
            failure: function () {
                Ext.Msg.alert('Error', 'Failed to update loan.');
            }
        });
    },
    deleteLoan: function (loanId) {
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/delete',
            method: 'DELETE',
            jsonData: {
                loanId: loanId
            },
            success: function (response) {
                Ext.Msg.alert('Success', 'Loan deleted successfully.', function () {
                    window.location.reload();
                },);
            },
            failure: function (response) {
                Ext.Msg.alert('Error', 'Failed to delete loan.');
            },
            scope: this
        });
    }
});
