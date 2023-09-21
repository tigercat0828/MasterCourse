Supervised learning learns an unknown mapping f through a set of (input $i$, output $o$) pairs
![[Pasted image 20230920233851.png]]

### Learning Process
Given $S$ and $H$, a learning algorithm. A attempts to find a hypothesis $h^\star\in H$ that minimizes the **empirical error** 
$$E(h)=\sum_{N}^{t=1}l(h(x^t)\ne r^t), h^\star =\mathop{\arg\min}_{\theta} E(h)$$ .![[Pasted image 20230920235317.png]]
## Learnability
3 Component in supervised learning process
- $S$ (sample)
- $H$ (hypothesis set)
- $A$ (learning algorithm)
**Probably approximately correct (PAC) learning**
the complexity of sample set $S$
**Vapnik-Chervonenkis (VC) dimension**
the complexity of hyothesis set $H$


## PAC learning
Answer to the sample complexity,
ex: the number of training examples required to achieve a satisied (PAC) answer.

Hypothses $s, g$ and Version Space $V$
Version space : $V = \{h | h$ \ is\ between $s\ and\ g\}$  
![[Pasted image 20230921000942.png]]

## PAC learnable
How many training example N should have the probability that selected hypothesis $h$ has error rate, $error(h) \le \epsilon$ at lest $1-\delta$ 
$$P(error(h) \le \epsilon) \ge 1-\delta$$
$h$ : hypothesis
$error(h)$ : the probability that h misclassifies a test example
$\epsilon$ : accuracy
$\delta$ : confidence


## VC dimension
A measure of hypothesis complexity : 
Infinite $|H|$ may have limited $VC(H)$
$\forall$ labeling , $\exists h \in H$ that separates + from - examples, $H$ shatters $N$ points.
![[Pasted image 20230921002554.png]]
ex: "Line" hypothesis class can shatter 3 points in the 2D space.
$VC(H)$ : the maximum number of points that can be shattered by $H$
$VC(line)=3$
$VC(AA-rectangle) = 4$
$VC(triangle) = 7$
![[Pasted image 20230921002751.png]]![[Pasted image 20230921002855.png]]

## Term

**Training Set**
**Validation Set**
**Test set**
**K-fold cross-validation**
**Ill-posed problem**
**Inductive bias**
**Model selection**
**Underfitting**
**Overfitting**
**Triple trade-off**


