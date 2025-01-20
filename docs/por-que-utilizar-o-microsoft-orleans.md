# Por que utilizar o Microsoft Orleans

## Introdução

Quando temos uma API, atrás de um balanceador de carga, é possível ter diversas tarefas (**ECS tasks**) de um determinado serviço, em um *cluster* (**ECS cluster**), numa conta da **AWS**, e assim, podemos ter alta disponibilidade e escalabilidade. Outros cenários podem ser resolvidos através de utilização de **Lambda Functions**. No entanto, se tivermos um serviço do tipo *worker*, em **C#**, baseado no <a href="https://learn.microsoft.com/pt-br/dotnet/core/extensions/windows-service" target="_blank">BackgroundService</a>, que realiza determinado processamento em um intervalo de tempo, como podemos ter alta disponibilidade e escalabilidade? Se definirmos, por exemplo, 2 tarefas, na tentativa de resolver essas questões, os processamentos poderão ocorrer em duplicidade. Se criamos um mecanismo, onde mesmo que o processamento ocorra em duplicidade, o resultado não seja duplicado, não estaremos sendo eficiente no que se refere a custos e não será possível escalar horizontalmente, porque não importa a quantidade de tarefas, todas sempre vão ter a mesma carga de processamento. Se criarmos algum mecanismo para que apenas uma das tarefas realize os processamentos enquanto a outra fique em *stand by*, caso a primeira tarefa fique indisponível por qualquer motivo, de novo, vamos gastar o dobro que seria necessário, além disso não será possível escalar horizontalmente.

Este é um dos cenários onde o **Microsoft Orleans** se mostra necessário, com esse framework é possível ter diversas instâncias de um mesmo serviço, com processamentos únicos, onde essas instâncias distribuem a carga entre si, tendo assim, uma aplicação com alta disponibilidade e escalabilidade.

## O que é Microsoft Orleans?

O **Microsoft Orleans** é um *framework* projetado para simplificar a construção de sistemas distribuídos e escaláveis, sendo especialmente útil para aplicações que precisam lidar com alta concorrência e gerenciar estado de forma eficiente. Baseado no modelo de ator virtual, o Orleans introduz o conceito de "grãos" (*grains*), que são unidades de lógica e estado, permitindo que os desenvolvedores criem aplicações distribuídas sem se preocupar diretamente com a complexidade de sincronização, localização de recursos e concorrência.

Entre seus principais benefícios estão a escalabilidade horizontal transparente, o balanceamento dinâmico de carga e a integração com persistência de estado, o que facilita o desenvolvimento de sistemas resilientes e de alta performance. Além disso, o **Orleans** utiliza conceitos familiares da <a href="https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/tutorials/oop" target="_blank">Programação Orientada a Objetos</a> (POO), tornando-se acessível mesmo para quem não possui experiência prévia com sistemas distribuídos.

Com aplicações em jogos *online*, processamento em tempo real, sistemas *IoT* e muito mais, o **Microsoft Orleans** é uma ferramenta poderosa para quem busca criar sistemas robustos sem "reinventar a roda". Essa abordagem tem atraído desenvolvedores que buscam simplicidade e produtividade, oferecendo uma alternativa moderna e eficiente para resolver desafios complexos de distribuição e escalabilidade.

## Criação do Microsoft Orleans

O **Microsoft Orleans** foi criado para atender às necessidades específicas da Microsoft na construção de sistemas distribuídos para aplicações massivamente escaláveis, como jogos online e serviços de *backend* em larga escala. Ele surgiu como uma resposta aos desafios enfrentados ao usar abordagens tradicionais de sistemas distribuídos, que muitas vezes exigem soluções customizadas e complexas para lidar com problemas de concorrência, estado distribuído e escalabilidade.

A principal inspiração por trás do **Orleans** foi o modelo de ator, um paradigma de computação distribuída em que "atores" representam unidades independentes de lógica e estado que se comunicam entre si por meio de mensagens assíncronas. Este modelo, inicialmente popularizado por *frameworks* como o *Erlang*, provou ser uma solução eficaz para sistemas distribuídos, mas a equipe do Orleans o adaptou para simplificar ainda mais o desenvolvimento, criando o conceito de ator virtual.

No **Orleans**, os atores são chamados de grãos (*grains*), e seu modelo elimina a necessidade de os desenvolvedores gerenciarem manualmente a ativação, a localização e a concorrência entre os atores. Cada grão é ativado automaticamente sob demanda, com seu estado gerenciado de forma transparente, enquanto o *runtime* do **Orleans** cuida de detalhes como distribuição, persistência e balanceamento de carga. Isso reduz drasticamente a complexidade do desenvolvimento e oferece uma abordagem mais acessível para criar sistemas distribuídos em larga escala.

Essa abordagem foi testada e refinada em aplicações de alta performance, como o backend do jogo ***Halo***, demonstrando sua eficácia no gerenciamento de milhões de jogadores simultâneos.

## Grãos e Silos

No **Microsoft Orleans**, grãos e silos são conceitos fundamentais que estruturam a arquitetura do framework para lidar com sistemas distribuídos. Aqui está uma explicação de cada um:

### Grãos (*Grains*):

Os grãos são as unidades fundamentais de lógica e estado no **Orleans**, representando "atores virtuais". Eles são objetos que encapsulam tanto o comportamento quanto o estado da aplicação, funcionando de forma independente.

> #### Principais características dos Grãos:
>
> - **Ator Virtual:** Diferente do modelo tradicional de ator, os grãos não têm uma instância fixa em memória. Eles são ativados automaticamente sob demanda e podem ser descarregados quando inativos.
>   
> - **Estado Persistente:** Podem armazenar seu estado em repositórios persistentes, como bancos de dados, sem que o desenvolvedor precise gerenciar isso manualmente.
>   
> - **Comunicação Assíncrona:** Os grãos se comunicam entre si usando mensagens assíncronas, o que facilita a criação de sistemas distribuídos que escalam de forma transparente.
>   
> - **Exemplo de uso:** Em um jogo multiplayer, um grão pode representar uma entidade como um jogador, um item ou uma sala de jogo, com cada um deles gerenciando seu próprio estado e lógica.
>   

### Silos:

Os silos são os contêineres que hospedam os grãos, proporcionando o ambiente necessário para que eles funcionem. Pense neles como os "nós" físicos ou virtuais que compõem a infraestrutura de um sistema **Orleans**. 

> #### Principais características do Silos: 
>
> - **Execução Distribuída:** Cada silo executa grãos e cuida da comunicação entre grãos localizados em diferentes silos.
>   
> - **Escalabilidade:** Novos silos podem ser adicionados dinamicamente para expandir o sistema, permitindo que ele escale horizontalmente.
>
> - **Gerenciamento de Grãos:** Os silos são responsáveis por ativar, desativar e localizar grãos conforme necessário.
>
>  - **Resiliência:** Silos interagem para garantir que falhas em um nó não comprometam o sistema como um todo.

### Relação entre Grãos e Silos:

Os grãos vivem dentro dos silos. Quando um grão precisa ser chamado, o silo cuida de ativá-lo e de armazenar seu estado em memória ou persistência. Vários silos podem trabalhar juntos para distribuir a carga de trabalho e garantir a alta disponibilidade e a escalabilidade do sistema. 

Um grupo de silos é conhecido como um *cluster*. Você pode organizar seus dados armazenando diferentes tipos de grãos em diferentes silos. Seu aplicativo pode recuperar grãos individuais sem precisar se preocupar com os detalhes de como elas são gerenciados dentro do cluster.

![Cluster com silos (hosts) e grãos](./img/cluster-silo-grain-relationship.svg "Cluster com silos (hosts) e grãos")

#### Exemplo Simples:

•	**Grão:** Em uma aplicação de gerenciamento de pedidos, um grão pode representar um pedido específico, contendo detalhes como itens, status e histórico.

•	**Silo:** O servidor ou cluster de servidores onde esses grãos são hospedados e gerenciados.

Na parte final deste artigo aplicaremos o **MS Orleans** numa aplicação funcional.

### Ciclo de vida do grão

![Ciclo de vida do grão, desde sua ativação até a desativação](./img/grain-lifecycle.svg "Ciclo de vida do grão, desde sua ativação até a desativação")
 
Esta imagem ilustra o ciclo de vida de um grão no **Microsoft Orleans**. Cada grão pode passar por diferentes estados ao longo de sua existência no sistema. Aqui está a explicação dos estados e transições representados:

<ol>
  <li><b><i>Activating</i> (Ativando):</b>
   <ul>
      <li><p>Quando um grão é acessado pela primeira vez ou quando precisa ser usado novamente, ele entra no estado de ativação.</p></li>
      <li><p>Durante este estado, o **Orleans** inicializa o grão, carregando-o na memória e restaurando seu estado persistente (se necessário).</p></li>
    </ul>
  </li>
  <li><b><i>Active in Memory</i> (Ativo na Memória):</b>
  <ul>
      <li><p>Após a ativação, o grão fica carregado na memória.</p></li>
      <li><p>Neste estado, o grão está pronto para processar solicitações de outros grãos ou de clientes.</p></li>
      <li><p>Ele permanece neste estado enquanto for necessário ou até que se torne inativo.</p></li>
    </ul>
  </li>
  <li><b><i>Deactivating</i> (Desativando):</b>
    <ul>
      <li><p>Quando o grão não é mais necessário, o **Orleans** inicia o processo de desativação.</p></li>
      <li><p>Durante este estado, o *framework* pode persistir o estado do grão (se for configurado para isso) antes de removê-lo da memória.</p></li>
    </ul>
  </li>
  <li><b><i>Persisted</i> (Persistido):</b>
    <ul>
       <li><p>Após a desativação, o estado do grão pode ser salvo em um armazenamento persistente (por exemplo, um banco de dados ou armazenamento em nuvem).</p></li>
       <li><p>O grão permanece persistido até que seja necessário novamente.</p></li>
    </ul>
  </li>
</ol>

#### Fluxo Geral:

- Um grão passa por ***"Activating"*** → ***"Active in Memory"*** quando é necessário.

- Quando o grão não está mais ativo, ele passa por "Deactivating" → "Persisted".

-	Se o grão persistido for requisitado novamente, ele retorna ao ciclo de ativação.

<br>

> #### Objetivo do Ciclo de Vida:
> Esse ciclo permite que o Orleans gerencie recursos de forma eficiente em um sistema distribuído. Ele garante que:
> 
> -	Grãos inativos não consumam memória.
>   
> - O estado do grão possa ser restaurado sempre que necessário.
>   
> - O sistema possa escalar dinamicamente, ativando e desativando grãos conforme a demanda.
> 
> Essa abordagem baseada no ciclo de vida automatiza o gerenciamento de estado e memória, facilitando a criação de aplicações distribuídas escaláveis e resilientes.


## Exemplo Prático



http client https://learn.microsoft.com/pt-br/dotnet/api/system.net.http.httpclient?view=net-8.0



https://redis.io/try-free/




