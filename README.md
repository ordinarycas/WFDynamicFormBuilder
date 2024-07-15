# WFDynamicFormBuilder (DynamicFormFW)

WFDynamicFormBuilder �O�@�Өϥ� C# 4.8 WebForms ���ʺA���c�ؾ��A���\�Τ�ۥѳ]�p���D�M����A������ Google ���C�o�ӱM�״��ѤF�@�ө���ϥΪ������ӳЫءB�޲z�M��V�i�۩w�q�����C

## �\��(Features)

- **�h�ر������**�G�]�A�奻�ءB�����s�B�ƿ�ءB�U�Կ�浥�C
- **��ɪ��w��**�G�b�c�ت��ɧY�ɬd�ݹw���C

## �t��(Dome)

![image](./DemoData/WFDynamicFormBuilder.gif)

## Entity Relationship Diagram

```mermaid
erDiagram
    FormBasicInfo ||--o{ FormBlock : FormId
    FormBasicInfo ||--o{ FormControl : "FormId"
    FormBasicInfo {
        int Id PK
        string Name
        string Description
        int Version
        datetime AtCreatDateTime
        string EmpNo
    }

    FormBlock ||--o{ FormControl : "FormBlockId"
    FormBlock {
        int FormId
        int Id
        string Title
        int ColCount
        int Version
        datetime AtCreatDateTime
        datetime AtLastDateTime
    }

    FormControl {
        int FormId PK
        int Version
        int BlockId
        int BlockColIndex
        string ControlId
        string ControlType
        string ControlTitle
        datetime AtCreatDateTime
        datetime AtLastDateTime
    }
```

## �}�l�ϥ�(Getting Started)

### ���һݨD(Prerequisites)

- Visual Studio 2019 or later
- .NET Framework 4.8
- Basic knowledge of C# and WebForms
- MSSQL

### �w�ˤ覡(Installation)

�ݭn����ƪ��� DemoData


## �^�m(Contributing)

Contributions are welcome! Please fork the repository and create a pull request with your changes.

�w��^�m�I�� fork �o�ӭܮw�óЫ� pull request ����A�����C