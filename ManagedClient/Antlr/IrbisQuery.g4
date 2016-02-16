//============================================================
// ���������� ���� ��������� �������� �����
// ���������� ��� ANTLR 4.2
// �����: �. �. �������
// ������: 0.0.6
//============================================================
 
// �������� ����������� ����������
// http://wiki.elnit.org/index.php/����_��������_�����
 
grammar IrbisQuery;

// ��������� ������
program
        : levelThree EOF
        ;
 
// ������� �������, �� ������� ����� ���� ������� � �����������
// ���������� �������
levelThree
        : levelTwo                                          # levelTwoOuter
        | REFERENCE                                         # reference
        | left=levelThree op=(STAR|HAT) right=levelThree    # starOperator3
        | left=levelThree PLUS right=levelThree             # plusOperator3
        ;
 
// ��������� � ������� ����� ������������ ������ ����������� PLUS STAR HAT.
 
// ������������� �������, �� ������� ����������� ����������� ���������,
// �� ��������� ������
levelTwo
        : levelOne                                      # levelOneOuter
        | LPAREN levelTwo RPAREN                        # parenOuter
        | left=levelTwo op=(STAR|HAT) right=levelTwo    # starOperator2
        | left=levelTwo PLUS right=levelTwo             # plusOperator2
        ;
 
// �������� �������� �� ���������� ����������:
// DOT
// F
// G
// STAR � HAT
// PLUS
 
// ����� ������ �������, �� ������� �������� ����������� ���������
// (������� ����������� ������)
levelOne
        : ENTRY                                         # entry
        | left=levelOne DOT right=levelOne              # dotOperator
        | left=levelOne F   right=levelOne              # fOperator
        | left=levelOne G   right=levelOne              # gOperator
        | left=levelOne op=(STAR|HAT) right=levelOne    # starOperator1
        | left=levelOne PLUS right=levelOne             # plusOperator1
        ;
 
ENTRY
        : (QUOTED|NONQUOTED) (SLASH LPAREN TAGNUMBER (COMMA TAGNUMBER)* RPAREN)?
        ;
 
// ������ � ������� ��������
QUOTED  : '"' .*? '"';
 
// �������������� ������
// \u0400-\u04FF - ������������� �������
NONQUOTED
        : [0-9A-Za-z\u0400-\u04FF=\[\]~!@$%&_'-] [0-9A-Za-z\u0400-\u04FF=\[\]~!@#$%&_'-]+
        ;
 
// ������ �� ���������� ����������� ������
REFERENCE : '#' [0-9]+;
 
// ���� ������
TAGNUMBER : [0-9]+;
 
// ���������
 
PLUS    : '+';   // ���������� ���
STAR    : '*';   // ������� ���������� �
HAT     : '^';   // ���������� ��
G       : '(G)'; // ����������� � (� ����� ����)
F       : '(F)'; // ����������� � (� ����� ����������)
DOT     : '.';   // ����������� � (����� ������)
 
// ��������� �������
 
LPAREN  : '(';
RPAREN  : ')';
SLASH   : '/';
COMMA   : ',';
 
// �������
WS: [ \t\r\n]+ -> skip;