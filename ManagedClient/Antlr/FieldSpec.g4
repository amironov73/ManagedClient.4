/* Спецификация отбора полей для ИРБИС.
 */

grammar FieldSpec;

program
            :   include
                exclude?
                EOF
            ;

include
            :   specItem+;

exclude
            :   BANG
                specItem+
            ;

specItem    :   EXPR
            ;

BANG        :   '!' ;
DELIMITER   :   [ \t\r\n;,]+ -> skip;
EXPR        :   [0-9xX\[\]]+;
MINUS       :   '-' ;

