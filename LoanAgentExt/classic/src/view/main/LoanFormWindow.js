Ext.define('LoanAgentExt.view.main.LoanFormWindow', {
    extend: 'Ext.window.Window',
    xtype: 'loanformwindow',

    title: 'Create New Loan',
    modal: true,
    layout: 'fit',
    width: 400,

    items: [{
        xtype: 'form',
        bodyPadding: 10,
        defaults: {
            anchor: '100%',
            allowBlank: false
        },
        items: [
            {
                xtype: 'numberfield',
                fieldLabel: 'Loan Amount',
                name: 'loanAmount',
                minValue: 0
            },
            {
                xtype: 'combo',
                fieldLabel: 'Currency',
                name: 'currency',
                store: [
                    { text: 'Gel', value: 'Gel' },
                    { text: 'Usd', value: 'Usd' },
                    { text: 'Eur', value: 'Eur' }
                ],
                queryMode: 'local',
                displayField: 'text',
                valueField: 'value',
                editable: false,
                value: 'Gel'
            },
            {
                xtype: 'combo',
                fieldLabel: 'Loan Type',
                name: 'loanType',
                store: [
                    { text: 'QuickLoan', value: 'QuickLoan' },
                    { text: 'CarLoan', value: 'CarLoan' },
                    { text: 'Installment', value: 'Installment' }
                ],
                queryMode: 'local',
                displayField: 'text',
                valueField: 'value',
                editable: false,
                value: 'QuickLoan'
            },
            {
                xtype: 'datefield',
                fieldLabel: 'Start Date',
                name: 'startDate',
                format: 'Y-m-d'
            },
            {
                xtype: 'datefield',
                fieldLabel: 'End Date',
                name: 'endDate',
                format: 'Y-m-d'
            }
        ],

        buttons: [
            {
                text: 'Create',
                formBind: true,
                disabled: true,
                handler: function (button) {
                    const form = button.up('form').getForm();
                    if (form.isValid()) {
                        button.up('window').submitLoan();
                    }
                }
            },
            {
                text: 'Cancel',
                handler: function (button) {
                    button.up('window').close();
                }
            }
        ]
    }],

    submitLoan: function () {
        const form = this.down('form').getForm();
        const values = form.getValues();

        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/create',
            method: 'POST',
            jsonData: values,
            success: function (response) {
                Ext.Msg.alert('Success', 'Loan created successfully!', function () {
                    window.location.reload();
                }, );
            },
            failure: function () {
                Ext.Msg.alert('Error', 'Failed to create loan.');
            },
            scope: this
        });
    }
});