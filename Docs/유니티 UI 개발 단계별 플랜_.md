

# **Unity 및 UI Toolkit을 활용한 프로젝트 구현을 위한 단계별 실행 계획서**

본 보고서는 Unity 엔진과 UI Toolkit 패키지를 사용하여 새로운 프로젝트를 구현하기 위한 포괄적이고 단계별 실행 계획을 제시합니다. 이 계획은 초기 환경 설정부터 시작하여 견고한 아키텍처 설계, 핵심 기능 구현, 그리고 최종 최적화 및 폴리싱 단계에 이르기까지 전체 개발 수명 주기를 다룹니다. 각 단계에서는 구체적인 구현 방법론, 코드 예시, 그리고 장기적인 프로젝트의 안정성과 확장성을 보장하기 위한 핵심 주의사항을 상세히 기술합니다. 이 문서는 단순한 기술 나열을 넘어, 각 결정이 프로젝트 전체에 미치는 영향을 심층적으로 분석하여 기술적 우수성을 추구하는 개발팀을 위한 전문 엔지니어링 가이드 역할을 하는 것을 목표로 합니다.

## **1\. 프로젝트 기반 공사: 환경 및 버전 관리**

프로젝트의 장기적인 성공과 팀 협업의 효율성은 이 초기 단계에 달려있습니다. 여기서 발생하는 오류나 단축된 절차는 심각한 기술 부채, 병합 충돌, 그리고 확장이 어려운 비조직적인 코드베이스로 이어질 수 있습니다. 이 섹션에서는 전문가 수준의 프로젝트 기반을 구축하는 방법을 설명합니다.

### **1.1. Unity 프로젝트 초기화 및 환경 구성**

개발의 첫 단추는 Unity 프로젝트를 생성하고 버전 관리 시스템과의 원활한 연동을 위해 핵심적인 에디터 설정을 구성하는 것입니다. 이 과정은 프로젝트의 모든 에셋이 일관되고 예측 가능한 방식으로 관리되도록 보장하는 기반이 됩니다.

#### **방법론**

Unity Hub를 통해 새 프로젝트를 생성하는 것으로 시작합니다. Universal 2D (URP)와 같은 템플릿 선택은 시작점일 뿐이며, 추후 변경이 가능합니다.1 가장 중요한 것은 생성 직후 버전 관리 시스템과의 호환성을 위해 에디터 설정을 즉시 구성하는 것입니다.

#### **구체적인 구현 방법**

1. Unity Hub를 열고 최신 Unity 6 LTS 버전을 사용하여 새 프로젝트를 생성합니다.  
2. 프로젝트가 열리면, 즉시 Edit \> Project Settings로 이동하여 Editor 탭을 선택합니다.  
3. Asset Serialization 항목에서 Mode를 Force Text로 설정합니다. 이는 버전 관리 시스템을 위한 필수 불가결한 설정으로, 씬(.unity)과 프리팹(.prefab) 파일을 사람이 읽을 수 있고 병합 가능한 텍스트 형식으로 저장합니다.2 바이너리 직렬화 방식은 병합 충돌 발생 시 해결을 거의 불가능하게 만듭니다.  
4. Version Control 항목에서 Mode를 Visible Meta Files로 설정합니다. .meta 파일은 에셋의 임포트 설정과 고유 ID(GUID)를 저장하는 핵심 파일입니다. 이 파일이 숨겨지거나 누락되면 프로젝트 내 모든 에셋 참조가 깨질 수 있습니다.2

#### **주의점**

팀 전체가 동일한 Unity 에디터 버전을 사용하는 것이 매우 중요합니다. 마이너 버전이나 패치 버전이 일치하지 않을 경우, 특정 팀원의 에디터가 에셋 메타데이터를 자동으로 재생성하여 버전 관리 시스템에 불필요한 변경 사항을 유발하고, 심각한 경우 씬과 프리팹의 참조를 깨뜨릴 수 있습니다.3

### **1.2. Git을 이용한 강력한 버전 관리 시스템 구축**

효과적인 버전 관리는 현대 소프트웨어 개발의 핵심입니다. Git을 올바르게 설정하고 Unity 프로젝트의 특성을 이해하여 구성하는 것은 협업의 효율성을 극대화하고 잠재적인 문제를 예방하는 데 필수적입니다.

#### **1.2.1. 저장소 설정 및 GitHub 연동**

Git 저장소는 Unity 프로젝트 폴더의 루트에서 초기화되어야 합니다. 이는 명령줄 인터페이스(CLI)나 GitHub Desktop과 같은 GUI 클라이언트를 통해 수행할 수 있습니다.4

#### **구체적인 구현 방법**

1. GitHub와 같은 원격 저장소 서비스에서 새로운 빈 저장소(repository)를 생성합니다. 이 과정에서 가장 중요한 단계는 **Unity용 .gitignore 템플릿을 선택하는 것**입니다.3 이는 가장 쉽고 정확하게 초기 설정을 시작하는 방법입니다.  
2. 생성된 빈 원격 저장소를 로컬 컴퓨터에 복제(clone)합니다.  
3. 복제된 폴더 **내부**에 새로운 Unity 프로젝트를 생성하거나, 기존 프로젝트의 전체 내용을 이 폴더로 이동시킵니다.3 이 워크플로우는 저장소의 루트가 Unity 프로젝트 자체가 되도록 보장하여, 프로젝트를 하위 폴더에 두는 구조보다 관리가 용이합니다.  
4. Unity 프로젝트 파일들을 추가하고 첫 번째 커밋(initial commit)을 생성합니다. 이 커밋에는 .gitignore 파일에 의해 제외된 파일들을 제외한 모든 프로젝트 구조가 포함됩니다.

#### **1.2.2. Unity 프로젝트에서 .gitignore의 핵심적 역할**

.gitignore 파일은 Git에게 어떤 파일과 디렉토리를 추적에서 제외할지 알려주는 단순 텍스트 파일입니다. Unity 프로젝트의 경우, 이는 자동으로 생성되는 대용량 캐시 파일이나 사용자별 설정 파일들을 버전 관리에서 제외하기 위해 필수적입니다.2

#### **구체적인 구현 방법**

* GitHub에서 제공하는 표준 Unity .gitignore 템플릿을 기반으로 사용해야 합니다.11 이 템플릿은  
  Library/, Temp/, Logs/, Build/, UserSettings/와 같은 폴더들을 올바르게 무시하도록 설정되어 있습니다.2 이 폴더들은 각 사용자의 로컬 머신에 특화된 캐시, 임시 빌드 파일, 개인 에디터 레이아웃 등을 포함하므로 팀원 간에 공유되어서는 안 되며 병합 충돌의 주요 원인이 됩니다.  
* 가장 중요한 점은 .meta 파일을 **절대 무시해서는 안 된다**는 것입니다. 이 파일들은 에셋 참조를 추적하며 프로젝트의 무결성에 필수적입니다.3

#### **주의점**

만약 적절한 .gitignore 파일 없이 저장소를 초기화하고 커밋했다면, 이러한 캐시 폴더들이 실수로 저장소에 포함될 수 있습니다. Git 히스토리에서 이를 제거하는 것은 복잡하고 오류가 발생하기 쉬운 과정입니다. 이런 경우, 히스토리를 정리하려 시도하기보다 저장소를 새로 시작하는 것이 더 빠르고 안전한 해결책일 수 있습니다.3

#### **1.2.3. 대용량 에셋 관리를 위한 Git LFS 설정**

Git은 텍스트 파일 관리에 최적화되어 있으며, 텍스처, 모델, 오디오 파일과 같은 대용량 바이너리 에셋을 처리하는 데는 비효율적입니다. Git LFS(Large File Storage)는 이러한 대용량 파일을 저장소 내에서는 작은 텍스트 포인터로 대체하고, 실제 파일 데이터는 별도의 서버에 저장하는 확장 기능입니다.5 이를 통해 핵심 저장소는 작고 빠르게 유지됩니다.

#### **구체적인 구현 방법**

1. 팀의 모든 구성원이 각자의 컴퓨터에 Git LFS 명령줄 확장 프로그램을 설치해야 합니다.14  
2. 프로젝트 저장소의 루트 디렉토리에서 터미널을 열고 git lfs install 명령을 실행합니다. 이 작업은 저장소당 한 번만 수행하면 됩니다.17  
3. LFS가 추적할 파일 유형을 정의해야 합니다. 이는 .gitattributes 파일에서 이루어집니다. git lfs track "\*.png"와 같이 개별적으로 파일을 추적하는 대신, Unity용으로 미리 만들어진 .gitattributes 템플릿을 사용하는 것이 가장 좋은 방법입니다. 이 템플릿은 일반적인 대용량 에셋 유형을 모두 포함하고 있습니다.3  
4. .gitattributes 파일은 **대용량 에셋을 추가하기 전에** 먼저 저장소에 커밋되어야 합니다. 이는 해당 에셋들이 처음부터 LFS에 의해 올바르게 처리되도록 보장합니다.15

#### **주의점**

GitHub와 같은 Git 호스팅 서비스는 LFS 저장 공간과 대역폭에 대해 무료 제공량에 제한이 있습니다(예: 1GB 저장 공간, 월 1GB 대역폭).17 프로젝트 규모가 커지면 유료 서비스로 전환해야 할 수 있으며, 이 비용은 프로젝트 예산에 반드시 고려되어야 합니다.

### **1.3. 확장 가능한 프로젝트 폴더 구조 설계**

잘 정의된 폴더 구조는 특히 팀 환경에서 프로젝트를 체계적으로 관리하는 데 매우 중요합니다. 단 하나의 "정답"은 없지만, 일관성을 유지하는 것이 핵심입니다.21 폴더 구조는 선택한 아키텍처 패턴(예: MVC)을 지원하고, 프로젝트 자체 에셋과 서드파티 에셋을 명확히 분리해야 합니다.

#### **구체적인 구현 방법**

* **최상위 레벨 구성:** 모든 프로젝트 관련 에셋을 담을 루트 폴더(예: \_Project/ 또는 \[ProjectName\]/)를 Assets 폴더 내에 생성합니다. 이는 Asset Store에서 임포트한 패키지들과 프로젝트 에셋을 분리하여 관리하는 데 도움을 줍니다.21 이 구조는 이름 충돌을 방지하고 서드파티 에셋 업데이트를 훨씬 깔끔하게 만듭니다.  
* **유형 기반 vs. 기능 기반 구조:**  
  * **유형 기반 (Type-Based):** 에셋을 유형별로 그룹화하는 일반적인 접근 방식입니다(예: Scripts/, Materials/, Prefabs/, Scenes/).12 이 방식은 소규모 프로젝트에 직관적이고 간단합니다.  
  * **기능 기반 (Feature-Based):** 대규모 프로젝트의 경우, 기능별로 에셋을 그룹화하는 것이 더 확장성이 높을 수 있습니다(예: Player/, Enemies/, UI/, InventorySystem/). 특정 기능과 관련된 모든 에셋이 한곳에 모여있어 관리하기 용이합니다. 종종 두 방식을 혼합한 하이브리드 접근법이 최상의 결과를 낳습니다.  
* **명명 규칙 (Naming Conventions):** 명확하고 엄격한 명명 규칙을 설정하고 문서화해야 합니다. 파일 및 폴더 이름에 공백 사용을 피하고, 대신 파스칼 케이스(PascalCase)나 카멜 케이스(camelCase)를 사용하십시오. 공백은 명령줄 도구에서 문제를 일으킬 수 있습니다.21

#### **주의점**

Unity 에디터 내에서 에셋을 이동하는 것이 매우 중요합니다. 파일 시스템에서 직접 파일을 옮기면 해당 .meta 파일이 업데이트되지 않아, 그 에셋에 대한 모든 참조가 깨지게 됩니다. Git과 같은 버전 관리 시스템은 이를 파일의 삭제와 새로운 파일의 추가로 인식하여 해당 파일의 변경 이력을 유실하게 됩니다.21

이러한 초기 설정 단계들은 단순한 관리 작업이 아니라, 프로젝트의 기술적 건전성을 좌우하는 근본적인 아키텍처 결정입니다. 직렬화 방식, .gitignore 설정, Git LFS 도입은 저장소 팽창, 해결 불가능한 병합 충돌, 깨진 에셋 참조와 같은 협업 게임 개발에서 가장 흔하고 비용이 많이 드는 문제들을 사전에 방지합니다. 즉, 올바른 초기 설정은 깨끗한 저장소, 효율적인 협업, 그리고 확장 가능한 프로젝트로 이어지는 직접적인 인과 관계를 형성합니다.

## **2\. 분리되고 유지보수 가능한 코어를 위한 아키텍처 청사진**

견고한 프로젝트 기반이 마련되었다면, 다음 단계는 소프트웨어 아키텍처를 정의하는 것입니다. 이는 섣부른 최적화가 아니라, 코드의 명확성을 증진하고, 의존성을 줄이며("스파게티 코드" 방지), 프로젝트의 전체 수명 주기에 걸쳐 테스트, 유지보수, 확장을 용이하게 만드는 패턴을 확립하는 과정입니다.25

### **2.1. 올바른 아키텍처 패턴 선택: 비교 분석**

Model-View-Controller (MVC), Model-View-Presenter (MVP), Model-View-ViewModel (MVVM)과 같은 아키텍처 패턴은 데이터(Model), 표현(View), 로직(Controller/Presenter/ViewModel)을 분리하기 위한 프레임워크를 제공합니다.25 어떤 패턴을 선택하는지는 UI와 게임 로직이 상호작용하는 방식에 중대한 영향을 미칩니다.

#### **구성 요소별 역할**

* **Model:** UI와 독립적으로 애플리케이션의 데이터와 핵심 로직을 나타냅니다. Unity의 UI 관련 클래스를 참조해서는 안 됩니다.25 예시:  
  Health와 Score 속성을 가진 PlayerStats 클래스.  
* **View:** Model의 시각적 표현입니다. 이 프로젝트에서는 UXML, USS, 그리고 데이터 표시 및 사용자 입력 전달을 담당하는 최소한의 C\# "코드 비하인드" 스크립트로 구성됩니다.25 UI Toolkit의  
  VisualElement들이 View의 핵심입니다.  
* **Controller/Presenter/ViewModel:** Model과 View를 연결하는 중재자입니다.  
  * **MVC:** Controller가 사용자 입력을 처리하고 Model을 업데이트합니다. View는 변경 사항을 감지하기 위해 Model을 직접 관찰합니다.25 이 직접적인 연결은 Unity 환경에서 관리하기 복잡할 수 있습니다.  
  * **MVP:** View는 더 수동적입니다. 사용자 입력을 받아 Presenter에게 전달합니다. Presenter는 Model을 업데이트한 다음, View에게 어떻게 자신을 업데이트할지 명시적으로 지시합니다.25 이는 View와 Model 간의 직접적인 연결을 끊어 테스트를 더 용이하게 만듭니다.  
  * **MVVM:** 데이터 바인딩 기능이 있는 UI를 위해 특별히 설계되었습니다. ViewModel은 Model의 데이터를 속성(property)으로 노출합니다. View는 이 속성들에 직접 바인딩됩니다. ViewModel의 데이터가 변경되면 View는 바인딩 시스템을 통해 자동으로 업데이트되며, 그 반대도 마찬가지입니다.26

#### **권장 사항 및 비교**

UI Toolkit 프로젝트, 특히 Unity 6의 향상된 데이터 바인딩 기능을 활용하는 경우, **MVVM이 가장 강력하고 현대적인 선택**입니다.32 MVVM은 UI와 데이터를 동기화하는 데 필요한 상용구 코드(boilerplate code)의 양을 극적으로 줄여줍니다. UI 업데이트에 대한 수동 제어를 선호한다면 MVP가 강력한 차선책이 될 수 있습니다. 전통적인 MVC는 일반적으로 Unity UI 워크플로우에 덜 적합합니다.25

아래 표는 각 아키텍처 패턴의 주요 특징과 UI Toolkit과의 시너지를 비교 분석한 것입니다.

| 특성 | MVC (Model-View-Controller) | MVP (Model-View-Presenter) | MVVM (Model-View-ViewModel) |
| :---- | :---- | :---- | :---- |
| **데이터 흐름** | View \<-\> Controller \-\> Model; View가 Model을 직접 관찰 | View \<-\> Presenter \-\> Model; Presenter가 View를 업데이트 | View \<-\> ViewModel \<-\> Model; 데이터 바인딩을 통해 자동 동기화 |
| **컴포넌트 결합도** | 높음: View가 Model을 직접 알아야 함 | 중간: View와 Model이 Presenter를 통해 분리됨 | 낮음: View와 ViewModel이 데이터 바인딩을 통해 느슨하게 결합됨 |
| **테스트 용이성** | 중간: Controller와 Model은 테스트 가능하나 View의 의존성 존재 | 높음: Presenter와 Model이 UI와 무관하여 단위 테스트가 용이함 | 매우 높음: ViewModel이 View에 대한 참조가 없어 테스트가 매우 용이함 |
| **UI Toolkit 시너지** | 낮음: 데이터 변경 감지를 위한 추가 코드가 필요함 26 | 좋음: 명시적인 업데이트 로직 구현에 적합 | 탁월함: 데이터 바인딩을 활용하여 UI 업데이트 코드를 최소화함 27 |
| **주요 사용 사례** | 간단한 UI 또는 전통적인 소프트웨어 아키텍처 | 복잡한 로직과 UI의 완전한 분리가 중요하고, 단위 테스트가 필수적인 경우 | 데이터 중심의 복잡한 UI, 상태 변경이 빈번하며 코드 양을 최소화하고자 할 때 최적 |

### **2.2. 싱글톤 패턴을 이용한 핵심 관리자(GameManager, UIManager) 구현**

싱글톤 패턴은 클래스의 인스턴스가 단 하나만 존재하도록 보장하고, 이에 대한 전역적인 접근점을 제공합니다.34 이는 게임 상태를 관리하는

GameManager나 UI 상태 및 내비게이션을 담당하는 UIManager와 같은 중앙 관리자 클래스에 유용합니다.

#### **구체적인 구현 방법**

* 각 관리자 클래스마다 패턴을 반복해서 작성하는 것을 피하기 위해, 재사용 가능한 제네릭 싱글톤 기반 클래스를 만드는 것이 효율적입니다. 씬 전환 시에도 유지되어야 하는 관리자를 위해 DontDestroyOnLoad를 사용하는 영속적인 버전도 필요할 수 있습니다.36  
* GameManager 예시 코드 37:  
  C\#  
  using UnityEngine;

  public class GameManager : MonoBehaviour  
  {  
      public static GameManager Instance { get; private set; }

      private void Awake()  
      {  
          if (Instance\!= null && Instance\!= this)  
          {  
              Destroy(this.gameObject);  
              return;  
          }

          Instance \= this;  
          DontDestroyOnLoad(this.gameObject);  
      }

      // 게임 상태 관련 로직 (예: 점수, 레벨, 게임 단계 등)  
  }

* UIManager는 현재 어떤 UI 패널이 열려 있는지와 같은 UI 관련 상태를 관리하며, 동일한 싱글톤 패턴을 따릅니다.40

#### **주의점**

싱글톤 패턴을 남용하면 코드가 서로 강하게 결합되어 테스트와 리팩토링이 어려워질 수 있습니다.34 진정으로 전역적인 시스템에만 제한적으로 사용해야 합니다. 특히 관리자 클래스에서 다른 시스템으로의 통신은 직접적인 메서드 호출보다는 이벤트를 통해 이루어지는 것이 이상적입니다.

### **2.3. 이벤트 기반 아키텍처를 통한 시스템 분리 (C\# Action vs. UnityEvent)**

관리자 클래스가 다른 객체의 메서드를 직접 호출하는 대신(예: UIManager.Instance.UpdateHealthBar(newHealth)), 이벤트 기반 시스템은 객체들이 직접적인 참조 없이 통신할 수 있게 해줍니다. "발행자(publisher)"가 이벤트를 발생시키면, 다수의 "구독자(subscriber)"가 이를 수신하여 반응합니다. 이는 옵저버 패턴(Observer Pattern)의 한 형태입니다.43

#### **구체적인 구현 방법**

* **C\# Action:** 컴파일 시간에 확인되는 가볍고, 빠르며, 타입-세이프(type-safe)한 델리게이트입니다. 코드 대 코드 간의 통신에 이상적입니다.46  
  C\#  
  // 체력 데이터를 관리하는 Model 클래스 (예: PlayerHealth)  
  public static class GameEvents  
  {  
      public static event System.Action\<int\> OnHealthChanged;

      public static void RaiseHealthChanged(int newHealth)  
      {  
          OnHealthChanged?.Invoke(newHealth);  
      }  
  }

  // 체력 바를 표시하는 View 스크립트 (예: HealthBarView)  
  private void OnEnable() \=\> GameEvents.OnHealthChanged \+= UpdateHealthDisplay;  
  private void OnDisable() \=\> GameEvents.OnHealthChanged \-= UpdateHealthDisplay;

  private void UpdateHealthDisplay(int newHealth)   
  {   
      //... UI 업데이트 로직...   
  }

* **UnityEvent:** Unity 인스펙터에서 설정할 수 있는 직렬화 가능한 이벤트입니다. 이를 통해 프로그래머가 아닌 디자이너도 코드를 작성하지 않고 응답을 연결할 수 있지만, 리플렉션(reflection)을 사용하므로 성능이 느리고 타입 안정성이 떨어집니다.46

#### **권장 사항**

핵심 시스템 간의 통신에는 C\# Action을 사용하십시오.47

UnityEvent는 인스펙터에서 디자이너가 간단한 연결(예: 버튼 클릭 시 사운드 효과 재생)을 할 수 있도록 이벤트를 노출하는 경우에만 제한적으로 사용하는 것이 좋습니다.46

### **2.4. ScriptableObject를 활용한 유연한 게임 데이터 및 설정 관리**

ScriptableObject는 스크립트 인스턴스와 독립적으로 대량의 공유 데이터를 저장할 수 있는 에셋 파일을 만들 수 있게 해주는 Unity 클래스입니다.51 이는 아이템 속성, 적 능력치, 레벨 구성, 게임 설정 등을 정의하는 데 완벽하며, 데이터를 사용하는

MonoBehaviour로부터 데이터를 분리합니다.53

#### **구체적인 구현 방법**

* ScriptableObject를 상속하고 \[CreateAssetMenu\] 속성을 사용하는 C\# 스크립트를 생성합니다.52  
  C\#  
  using UnityEngine;

  public class GameSettingsSO : ScriptableObject  
  {  
      public float playerSpeed;  
      public int startingHealth;  
      public string gameVersion;  
  }

* Assets \> Create 메뉴를 통해 이 에셋의 인스턴스를 생성할 수 있습니다. "EasySettings", "HardSettings"와 같이 여러 버전의 설정을 만들 수 있습니다.54  
* MonoBehaviour는 이 GameSettingsSO 에셋에 대한 public 참조를 가질 수 있으며, 인스펙터에서 할당받아 사용합니다.

#### **주의점**

런타임에 ScriptableObject의 데이터를 수정하면 디스크에 저장된 에셋 파일 자체가 변경됩니다.51 게임이 중지될 때 초기화되어야 하는 런타임 데이터의 경우,

Awake나 Start에서 ScriptableObject의 초기 값을 런타임 변수로 복사하여 사용하는 것이 가장 좋은 방법입니다.51

이러한 아키텍처 요소들을 결합하면 매우 강력하고 현대적인 개발 패러다임이 형성됩니다. 특히 **MVVM, 데이터 바인딩, 그리고 ScriptableObjects의 조합**은 고도로 분리되고 데이터 중심적인 아키텍처를 가능하게 합니다. ScriptableObject는 씬 계층이나 코드 로직과 완전히 분리된 데이터(Model)를 정의하는 견고한 방법을 제공하며, 디자이너는 프로그래머의 개입 없이 이 데이터 에셋을 생성하고 수정할 수 있습니다. ViewModel은 이 ScriptableObject를 참조하여 UI가 이해할 수 있는 형태로 데이터를 노출하는 다리 역할을 합니다. 마지막으로 UI Toolkit의 View는 데이터 바인딩을 통해 ViewModel의 속성에 직접 연결되어, 데이터의 변경이 최소한의 C\# 연결 코드만으로 UI에 자동으로 전파됩니다. 이 흐름(ScriptableObject (데이터) \-\> ViewModel (데이터 준비) \-\> View (데이터 표시))은 모든 데이터 변경에 대해 수동 업데이트가 필요했던 과거의 패턴에서 크게 진화한 것입니다.

## **3\. 개념에서 상호작용까지: 프로토타이핑과 입력 시스템**

이 섹션은 추상적인 아키텍처와 실질적인 게임플레이 사이의 간극을 메웁니다. 그레이박싱을 통해 애플리케이션의 구조를 신속하게 구성하고, Unity의 현대적인 입력 시스템을 통해 사용자와의 주된 상호작용 채널을 구축하는 데 중점을 둡니다.

### **3.1. 그레이박싱 단계: 프리미티브를 활용한 게임플레이 및 UI 흐름 프로토타이핑**

그레이박싱(또는 화이트박싱)은 간단한 기본 도형(큐브, 구, 평면 등)을 사용하여 레벨이나 UI 레이아웃의 기능적 프로토타입을 제작하는 기법입니다.56 최종 아트 에셋을 제작하는 데 드는 시간과 비용 없이 핵심 메커니즘, 흐름, 규모를 테스트하는 것이 목표입니다.59

#### **구체적인 구현 방법**

* Unity의 내장 3D 오브젝트(GameObject \> 3D Object)를 사용하여 벽, 플랫폼, 캐릭터, 적, UI 패널과 같은 핵심 요소를 표현합니다.57  
* 더 복잡한 형태가 필요할 경우, 패키지 관리자를 통해 사용할 수 있는 ProBuilder 패키지는 에디터 내 모델링에 매우 유용한 도구이며, 이 단계에 이상적입니다.59  
* 미학보다는 기능에 집중합니다. 간단한 색상의 머티리얼을 사용하여 오브젝트 유형을 구분할 수 있습니다(예: 플레이어는 파란색, 적은 빨간색, 정적 환경은 회색).57  
* 이 단계는 핵심 게임 루프와 UI 내비게이션을 구현하고 테스트하는 시기입니다. 예를 들어, 플레이어가 메인 메뉴에서 설정 화면으로 이동했다가 다시 돌아올 수 있는지, 플레이어의 이동 능력에 비해 게임 공간의 크기가 적절한지 등을 검증합니다.

#### **주의점**

세부 사항에 얽매이지 않도록 주의해야 합니다. 그레이박싱의 목적은 속도와 반복적인 개선입니다. 이 단계에서는 성능이 중요한 고려 사항이 아니며, 플레이스홀더 지오메트리는 나중에 최종 에셋으로 교체될 것입니다.59

### **3.2. 새로운 입력 시스템 구현: 액션 기반 접근법**

Unity의 새로운 입력 시스템은 기존의 입력 관리자를 대체하는 더 유연하고 강력하며 장치에 구애받지 않는 프레임워크입니다. 이는 "액션" 기반으로, "점프"나 "발사"와 같은 추상적인 액션을 정의한 다음, 이를 다양한 장치의 특정 컨트롤(예: 스페이스바, 게임패드의 'A' 버튼)에 바인딩하는 방식입니다.63 이를 통해 게임 로직이 입력 하드웨어로부터 분리됩니다.

#### **3.2.1. 입력 액션 에셋 생성 및 구성**

#### **구체적인 구현 방법**

1. 패키지 관리자에서 Input System 패키지를 설치합니다.66 메시지가 나타나면 프로젝트 설정에서 활성화하며, 이 과정에서 에디터 재시작이 필요합니다.  
   Active Input Handling을 Both로 설정하여 기존 시스템을 사용하는 다른 패키지와의 호환성을 유지하는 것이 좋습니다.66  
2. Assets \> Create \> Input Actions를 통해 Input Actions 에셋을 생성합니다.67  
3. 생성된 에셋을 열어 입력 액션 에디터를 엽니다. 관련 액션을 그룹화하기 위해 **액션 맵(Action Maps)**(예: "Player", "UI")을 정의합니다. 각 맵 내에 **액션(Actions)**(예: "Move", "Look", "Submit")을 정의합니다. 각 액션에 대해 \*\*바인딩(Bindings)\*\*을 정의하여 특정 컨트롤에 매핑합니다(예: "Move" 액션에 WASD 키를 위한 2D Vector 바인딩 추가).63  
4. 에셋의 인스펙터에서 "Generate C\# Class"를 활성화하고 적용(Apply) 버튼을 클릭합니다. 이는 액션에 대한 강력한 타입의 접근을 제공하는 래퍼 클래스를 생성하여 오류를 줄여줍니다.69

#### **3.2.2. 액션을 C\# 로직에 연결하기**

액션은 주로 두 가지 방식으로 처리할 수 있습니다: PlayerInput 컴포넌트를 통한 UnityEvent 기반 콜백 방식, 또는 생성된 C\# 클래스를 직접 인스턴스화하고 관리하여 더 많은 제어권을 갖는 방식입니다. 후자가 유연성 측면에서 더 권장됩니다.

#### **구체적인 구현 방법 (C\# 클래스 접근법)**

C\#

using UnityEngine;  
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour  
{  
    private PlayerInputActions playerInputActions;

    private void Awake()  
    {  
        playerInputActions \= new PlayerInputActions();  
    }

    private void OnEnable()  
    {  
        playerInputActions.Player.Enable();  
        playerInputActions.Player.Jump.performed \+= OnJumpPerformed;  
    }

    private void OnDisable()  
    {  
        playerInputActions.Player.Jump.performed \-= OnJumpPerformed;  
        playerInputActions.Player.Disable();  
    }

    // 이동과 같이 지속적인 액션은 Update에서 직접 값을 읽어옵니다 (폴링)  
    private void Update()  
    {  
        Vector2 moveInput \= playerInputActions.Player.Move.ReadValue\<Vector2\>();  
        //... 이동 로직 처리  
    }

    // 점프와 같이 단발성 액션은 이벤트 콜백을 사용합니다  
    private void OnJumpPerformed(InputAction.CallbackContext context)  
    {  
        Debug.Log("Jump action was performed\!");  
        //... 점프 로직 트리거  
    }  
}

이 접근법은 이벤트(performed, started, canceled)에 대한 콜백과 지속적인 상태(ReadValue\<T\>)에 대한 폴링을 함께 사용하는 일반적이고 효과적인 패턴입니다.64

#### **주의점**

액션 맵을 사용하기 전에 반드시 활성화(playerInputActions.Player.Enable())해야 합니다. 활성화되지 않은 액션 맵은 입력을 처리하지 않습니다.64

그레이박싱과 액션 기반 입력 시스템은 철학적으로 연결되어 있습니다. 그레이박싱이 게임의 *기능*에 집중하기 위해 *시각적 요소*를 추상화하는 것처럼, 입력 시스템은 *플레이어의 의도*("액션")에 집중하기 위해 *하드웨어*를 추상화합니다. 이 둘을 함께 사용하면, 특정 아트나 컨트롤 방식에 얽매이지 않고 매우 추상적이고 유연한 상태에서 핵심 게임플레이 루프를 구축하고 테스트할 수 있는 강력한 프로토타이핑 워크플로우가 만들어집니다. 예를 들어, 개발자는 "점프"라는 액션에 로직을 연결하고, 디자이너는 나중에 코드 수정 없이 해당 액션에 키보드와 게임패드 바인딩을 모두 추가할 수 있습니다. 이처럼 입력 추상화와 시각적 추상화는 서로를 강화하여, 메커니즘과 컨트롤 방식을 독립적으로 신속하게 변경할 수 있게 해줍니다.

## **4\. UI Toolkit 마스터하기: 구조, 스타일, 로직**

이 섹션에서는 UI Toolkit의 실제적인 적용 방법을 심층적으로 다룹니다. 구조를 위한 UXML, 스타일을 위한 USS, 로직을 위한 C\#이라는 세 가지 핵심 구성 요소를 다루고, UI Builder 도구가 이 워크플로우를 어떻게 통합하는지 보여줍니다.

### **4.1. UI Toolkit의 삼위일체: UXML, USS, C\#**

UI Toolkit은 웹 개발 기술에서 영감을 받아, 관심사를 세 가지 명확한 파일 유형으로 분리합니다.73 이러한 분리는 아티스트와 프로그래머 간의 협업을 용이하게 하고 병합 충돌 가능성을 크게 줄입니다.

* **UXML (.uxml):** HTML과 유사한 XML 기반 마크업 언어로, UI 요소의 계층 구조와 구조를 정의하는 데 사용됩니다.76  
* **USS (.uss):** CSS와 유사한 스타일시트 언어로, UI 요소의 시각적 외형(색상, 폰트, 레이아웃, 간격 등)을 정의하는 데 사용됩니다.79  
* **C\# (.cs):** 로직을 처리하고, 이벤트에 응답하며, 데이터를 조작하는 데 사용됩니다.74

#### **구체적인 구현 방법**

1. Assets \> Create \> UI Toolkit 메뉴를 통해 UI 에셋을 생성합니다. 주요 에셋은 UI Document (UXML)와 Style Sheet (USS)입니다.1  
2. 씬에서는 GameObject에 UI Document 컴포넌트를 추가합니다. 이 컴포넌트는 씬에서 렌더링할 UXML 파일(Source Asset)을 연결하는 역할을 합니다.1

### **4.2. UI Builder를 이용한 시각적 저작**

UI Builder는 UXML 및 USS 파일을 시각적으로 생성하고 편집하기 위한 도구입니다.32 계층 구조(Hierarchy), 뷰포트(Viewport), 인스펙터(Inspector), 그리고 표준 컨트롤 라이브러리(Library)를 갖춘 친숙한 인터페이스를 제공합니다.74

#### **구체적인 구현 방법**

1. Window \> UI Toolkit \> UI Builder를 선택하거나 UXML 에셋을 더블 클릭하여 UI Builder를 엽니다.74  
2. 라이브러리에서 VisualElement, Label, Button과 같은 컨트롤을 계층 구조나 뷰포트로 드래그 앤 드롭합니다.1  
3. 계층 구조에서 요소를 선택하여 인스펙터에서 속성(name, text 등)과 인라인 스타일을 수정합니다.1  
4. StyleSheets 패널에서 새로운 USS 파일과 선택자(selector)를 직접 생성하여 인라인 스타일에서 재사용 가능한 클래스로 전환할 수 있습니다.74

### **4.3. Flexbox 모델을 이용한 반응형 레이아웃 구축**

UI Toolkit의 레이아웃 엔진은 CSS Flexbox 모델의 일부를 기반으로 합니다.89 이를 통해 다양한 컨테이너 크기와 화면 해상도에 적응하는 유연하고 반응형인 레이아웃을 생성할 수 있습니다. 이는 정적인 앵커 기반 시스템에서 동적인 컨테이너 기반 시스템으로의 사고방식 전환을 요구하며, 이 전환이 UI Toolkit의 진정한 힘을 발휘하는 열쇠입니다.

#### **주요 Flexbox 속성**

* **Flex Direction:** 주 축의 방향을 결정합니다 (row는 수평, column은 수직).79  
* **Justify Content:** 주 축을 따라 아이템을 정렬합니다 (예: flex-start, center, space-between).89  
* **Align Items:** 교차 축(주 축에 수직인 축)을 따라 아이템을 정렬합니다.89  
* **Flex Grow:** 아이템이 사용 가능한 공간을 채우기 위해 늘어날 수 있는지 결정합니다. 값이 1이면 늘어나고, 0이면 늘어나지 않습니다.86

이러한 속성들은 부모 컨테이너에 설정하여 직계 자식들의 레이아웃을 제어하며, UI Builder의 인스펙터에서 구성됩니다.1

#### **주의점**

오버레이와 같이 특별한 경우가 아니라면 Position: Absolute 사용을 피해야 합니다. 절대 위치에 과도하게 의존하면 유연한 레이아웃 엔진의 장점을 무력화시키고, 유지보수가 어려운 깨지기 쉬운 UI를 만들게 됩니다.89

### **4.4. UI 동작 스크립팅: 요소 쿼리 및 이벤트 처리**

C\#을 통해 UI를 동적으로 제어하기 위해서는 먼저 시각적 트리에서 UI 요소에 대한 참조를 얻고, 그 다음 사용자 상호작용에 응답하기 위한 이벤트 콜백을 등록해야 합니다.

#### **4.4.1. UQuery (Q\<T\>, Query\<T\>)를 이용한 UI 요소 접근**

jQuery와 LINQ에서 영감을 받은 UQuery 시스템은 시각적 트리에서 요소를 찾는 데 사용됩니다.93

#### **구체적인 구현 방법**

1. UI Builder에서 코드에서 접근해야 할 요소에 고유한 name을 할당합니다 (예: "start-button", "player-name-label").74  
2. C\# 스크립트에서 UI Document의 루트 VisualElement에 대한 참조를 가져옵니다.  
3. Q\<T\>(name) 메서드를 사용하여 특정 이름과 타입을 가진 *첫 번째* 요소를 찾습니다. 이것이 가장 일반적인 사용 사례입니다.93  
4. 더 복잡한 쿼리나 모든 일치하는 요소 목록을 얻으려면 Query\<T\>(name, className)를 사용합니다.95

C\#

using UnityEngine;  
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour  
{  
    private Button startButton;  
    private Label playerNameLabel;

    void OnEnable()  
    {  
        var root \= GetComponent\<UIDocument\>().rootVisualElement;  
          
        // Q\<T\>는 일치하는 첫 번째 요소를 가져오는 약식 표현입니다.  
        startButton \= root.Q\<Button\>("start-button");  
        playerNameLabel \= root.Q\<Label\>("player-name-label");

        if (startButton\!= null)  
        {  
            // 버튼 클릭 이벤트 등록 등  
        }  
    }  
}

#### **주의점**

UQuery는 계층 구조를 순회하므로 성능에 영향을 줄 수 있습니다. Update 메서드에서 매 프레임 쿼리하는 대신, OnEnable이나 Start에서 한 번만 쿼리하고 그 참조를 캐싱하여 사용하는 것이 좋습니다.93

#### **4.4.2. 버튼 클릭 및 기타 콜백 구현**

사용자 상호작용은 이벤트 시스템을 통해 처리됩니다. VisualElement에서 특정 이벤트가 발생했을 때 실행될 콜백 메서드를 등록합니다.96

#### **구체적인 구현 방법**

* 가장 흔한 이벤트는 버튼 클릭입니다. 이는 버튼의 clicked 이벤트에 콜백을 등록하여 처리합니다.  
* PointerOverEvent(마우스 호버)나 PointerOutEvent와 같은 다른 이벤트도 사용할 수 있습니다.97

C\#

void OnEnable()  
{  
    var root \= GetComponent\<UIDocument\>().rootVisualElement;  
    startButton \= root.Q\<Button\>("start-button");

    if (startButton\!= null)  
    {  
        // 버튼이 클릭되었을 때 호출될 메서드를 등록합니다.  
        startButton.clicked \+= OnStartButtonPressed;  
    }  
}

void OnDisable()  
{  
    // 메모리 누수를 방지하기 위해 항상 콜백을 해제해야 합니다.  
    if (startButton\!= null)  
    {  
        startButton.clicked \-= OnStartButtonPressed;  
    }  
}

private void OnStartButtonPressed()  
{  
    Debug.Log("Start button was clicked\!");  
    // 이벤트 시스템을 통해 게임 시작 로직을 트리거합니다 (섹션 2.3 참조).  
}

#### **주의점**

특히 UI가 동적으로 생성되고 파괴되는 경우, OnDisable에서 콜백 등록을 해제하는 것은 오류와 메모리 누수를 방지하는 데 매우 중요합니다.97

## **5\. 세련된 경험을 위한 고급 UI Toolkit 기법**

기본적인 기능 위에, 이 섹션에서는 기능적인 UI를 전문적이고 역동적이며 세련된 사용자 경험으로 격상시키는 고급 UI Toolkit 기능들을 탐구합니다. 이러한 기술들은 살아 움직이는 듯한 느낌을 주고 유지보수가 효율적인 인터페이스를 만드는 데 매우 중요합니다.

### **5.1. 데이터 바인딩을 이용한 동적 UI**

데이터 바인딩은 데이터 소스(예: C\# 객체의 속성)와 UI 요소의 속성(예: Label의 텍스트) 사이에 직접적인 연결을 생성합니다. 데이터 소스가 변경되면 UI가 자동으로 업데이트되고, 그 반대도 마찬가지여서 수동 업데이트 코드가 필요 없어집니다.27 이는 MVVM 패턴의 핵심입니다.

#### **구체적인 구현 방법**

* **런타임 바인딩:** 모든 C\# 객체의 속성을 UI 컨트롤에 바인딩할 수 있습니다.98  
* **UI Builder에서의 설정:**  
  1. 바인딩 컨텍스트의 루트가 될 VisualElement를 선택합니다. 인스펙터에서 Data Source 컴포넌트를 추가하고, Type을 바인딩할 C\# 클래스(예: ViewModel)로 설정합니다.  
  2. 바인딩할 UI 요소(예: Label)를 선택하고, 인스펙터에서 Binding 섹션을 찾습니다.  
  3. Binding Path를 데이터 소스 클래스의 public 속성 이름(예: PlayerName)으로 설정합니다.  
  4. Binding Mode를 설정합니다(예: To UI, From UI, Two Way).101  
* **C\#에서의 설정:**  
  C\#  
  // ViewModel 또는 데이터 소스 클래스  
  public class PlayerViewModel   
  {  
      // 이 속성이 바인딩의 소스가 됩니다.  
      public string PlayerName { get; set; } \= "Default Name";  
  }

  // UI 컨트롤러 스크립트  
  void OnEnable()  
  {  
      var root \= GetComponent\<UIDocument\>().rootVisualElement;  
      var viewModel \= new PlayerViewModel();

      // 루트 요소에 바인딩 소스를 설정합니다.  
      root.dataSource \= viewModel;  
  }

  이제 코드에서 viewModel.PlayerName을 변경하면, PlayerName 경로에 바인딩된 모든 Label의 텍스트가 자동으로 업데이트됩니다.

#### **주의점**

UI Builder에서의 데이터 바인딩은 민감할 수 있습니다. Binding Path가 C\# 클래스의 속성 이름과 정확히 일치하는지 확인해야 합니다. 바인딩 관련 오류는 명시적이지 않을 수 있어 디버깅이 까다로울 수 있습니다.102 자식 요소의 바인딩이 올바르게 해석되려면 부모 요소에

dataSource가 설정되어 있어야 합니다.

### **5.2. 재사용 가능한 커스텀 컨트롤 제작**

캐릭터 초상화와 체력 바, 이름이 결합된 UI처럼 복잡하거나 자주 사용되는 UI 컴포넌트의 경우, 커스텀 VisualElement 서브클래스를 만드는 것이 효율적입니다. 이는 구조(UXML), 스타일(USS), 로직(C\#)을 하나의 재사용 가능한 컴포넌트로 캡슐화하여, UI Builder에서 표준 컨트롤처럼 사용할 수 있게 해줍니다.103

#### **구체적인 구현 방법**

1. VisualElement를 상속하는 C\# 클래스를 생성합니다.  
2. 이 클래스 내에 중첩된 UxmlFactory와 UxmlTraits 클래스를 정의합니다. 이는 커스텀 컨트롤을 UI Builder에 노출시키고 커스텀 UXML 속성을 정의할 수 있게 해줍니다.103  
3. 커스텀 컨트롤 클래스의 생성자에서, 해당 컨트롤의 내부 구조를 정의하는 전용 UXML 파일을 로드(.CloneTree())하고 특정 스타일시트를 첨부할 수 있습니다.  
4. 이렇게 생성된 커스텀 컨트롤은 UI Builder의 Library \> Project \> Custom Controls 섹션에 나타납니다.106

#### **주의점**

Unity는 Button과 같은 내장 컨트롤로부터 상속하는 것을 권장하지 않습니다. 내부 구조가 향후 Unity 버전에서 변경될 수 있기 때문입니다. VisualElement에서 상속받아 표준 요소들을 조합하여 컨트롤을 구성하는 것이 더 안전합니다.107

### **5.3. USS 트랜지션을 이용한 UI 애니메이션**

UI Toolkit은 CSS와 유사한 트랜지션 시스템을 포함하고 있어, 복잡한 애니메이션 컨트롤러나 C\# 코드 없이 USS에서 직접 간단한 애니메이션을 만들 수 있습니다. 이는 호버 효과, 페이드인, 기타 상태 변화에 이상적입니다.108

#### **구체적인 구현 방법**

* 트랜지션은 요소의 기본 상태에 정의됩니다. 애니메이션을 적용할 속성(transition-property), 지속 시간(transition-duration), 완화 함수(transition-timing-function), 지연 시간(transition-delay)을 지정합니다.97  
* 애니메이션은 일반적으로 :hover와 같은 의사 클래스(pseudo-class)가 적용되어 트랜지션 속성의 값이 변경될 때 트리거됩니다.78  
* **마우스를 올렸을 때 크기가 커지고 색상이 변하는 버튼의 USS 예시:**  
  CSS  
  /\* 버튼의 기본 상태 \*/

.my-button {  
background-color: blue;  
scale: 1.0 1.0;  
transition-property: background-color, scale;  
transition-duration: 0.3s;  
transition-timing-function: ease-out;  
}

/\* 호버 상태 \*/

.my-button:hover {  
background-color: lightblue;  
scale: 1.1 1.1;  
}  
\`\`\`

#### **주의점**

성능을 위해 레이아웃 재계산을 유발하지 않는 속성에 애니메이션을 적용하는 것이 가장 좋습니다. transform 속성(translate, rotate, scale)과 opacity는 애니메이션에 매우 최적화되어 있습니다. width, height, margin과 같은 속성에 애니메이션을 적용하면 레이아웃 엔진이 전체 UI를 재평가하게 되어 성능이 저하될 수 있습니다.109 복잡하고 다단계인 애니메이션의 경우, C\#에서 DOTween과 같은 트위닝 라이브러리를 사용하는 것이 여전히 더 나은 선택일 수 있습니다.113

### **5.4. 여러 해상도에 걸친 UI 반응성 보장**

반응형 UI는 다양한 화면 크기와 종횡비에 자연스럽게 적응합니다. 이는 Flexbox 레이아웃 엔진, 백분율 기반 크기 조정, 그리고 PanelSettings 에셋의 조합을 통해 달성됩니다.108

#### **구체적인 구현 방법**

* **Flexbox:** 섹션 4.3에서 다룬 바와 같이, Flexbox 속성(flex-grow, flex-shrink 등)을 사용하여 공간을 지능적으로 분배하는 유동적인 레이아웃을 만듭니다.  
* **백분율 크기 조정:** UI Builder에서 width와 height 속성을 픽셀(px) 대신 백분율(%)로 설정할 수 있습니다. 이는 요소가 부모 컨테이너에 상대적으로 크기를 조절하게 만듭니다.116  
* **PanelSettings 에셋:** 이 에셋은 UI Document의 전반적인 스케일링 동작을 제어합니다. Assets \> Create \> UI Toolkit \> Panel Settings Asset을 통해 생성합니다.  
* PanelSettings 에셋에서 Scale Mode를 Scale With Screen Size로 설정합니다. 이는 UGUI의 CanvasScaler와 유사하게 작동합니다.1  
  Reference Resolution(예: 1920x1080)을 제공하면, 실제 화면 해상도가 다를 경우 전체 UI가 비례적으로 확대/축소됩니다.119  
* Screen Match Mode는 다른 종횡비를 어떻게 처리할지 결정합니다.119

#### **테스트**

Game 뷰의 해상도 드롭다운 메뉴나 Device Simulator (Window \> General \> Device Simulator)를 사용하여 다양한 일반 해상도와 종횡비에서 UI를 테스트해야 합니다.123

이러한 고급 기능들은 개별적인 도구가 아니라, \*\*"살아있는 UI(Living UI)"\*\*를 구축하기 위한 유기적인 생태계를 형성합니다. 데이터 바인딩은 UI가 데이터의 상태를 단순히 '보여주는' 것이 아니라 데이터 그 자체가 되도록 만들고, 커스텀 컨트롤은 이러한 동적인 구성 요소를 재사용 가능한 'UI 언어'의 일부로 캡슐화합니다. USS 트랜지션은 상태 변화에 시각적인 생동감과 피드백("주스")을 더합니다. 이 모든 것이 결합되어, UI는 정적인 정보 표시판이 아닌, 동적이고 반응하며 자율적인 시스템으로 거듭납니다.

## **6\. 에셋 통합 및 성능 최적화**

마지막 섹션은 프로젝트를 프로덕션 준비 상태로 만드는 데 중점을 둡니다. 이는 그레이박스 플레이스홀더를 최종 아트 에셋으로 교체하는 깔끔한 워크플로우를 확립하고, 특히 UI 관련 병목 현상에 초점을 맞춰 애플리케이션의 성능을 체계적으로 최적화하는 과정을 포함합니다.

### **6.1. 전문적인 에셋 워크플로우: 플레이스홀더에서 최종 아트로**

그레이박스 프로토타입에서 최종 아트 에셋으로 전환하는 과정은 부드럽고 비파괴적이어야 합니다. 아티스트가 에셋을 업데이트할 때 프로그래머가 로직을 다시 연결할 필요가 없는 워크플로우를 구축해야 합니다.

#### **구체적인 구현 방법**

* **플레이스홀더 오브젝트:** 그레이박싱 단계에서는 기본 도형이나 간단한 ProBuilder 메시를 사용합니다.56 이러한 플레이스홀더는 자체  
  GameObject에 있어야 합니다.  
* **에셋 교체:** 플레이스홀더를 교체하려면, 플레이스홀더의 MeshRenderer를 비활성화하고 최종 3D 모델을 플레이스홀더 GameObject의 자식으로 추가합니다. 콜라이더나 상호작용 로직과 같은 모든 스크립트와 컴포넌트는 부모 플레이스홀더 오브젝트에 그대로 유지됩니다. 이렇게 하면 시각적 표현만 교체하면서 모든 로직을 보존할 수 있습니다.  
* **Unity Asset Manager:** 대규모 팀의 경우, Unity Asset Manager를 사용하여 에셋을 클라우드에 중앙 집중화하는 것을 고려할 수 있습니다. 이는 단일 진실 공급원(single source of truth)을 제공하며, 아티스트가 에셋 업데이트를 푸시하면 개발자가 에디터에서 원활하게 업데이트할 수 있도록 합니다.126

#### **주의점**

아티스트와 개발자 간에 에셋의 스케일, 방향, 피벗 포인트에 대한 명확한 소통과 표준을 유지해야 합니다. "모든 캐릭터는 키가 2 유닛이고 Z축을 정면으로 향한다"와 같은 표준을 설정하면 엔진 내에서 계속해서 조정해야 하는 번거로움을 줄일 수 있습니다.129

### **6.2. 에셋 최적화 심층 분석: 텍스처, 모델, 오디오**

에셋 최적화는 특히 모바일 및 웹 플랫폼에서 빌드 크기, 메모리 사용량, 로딩 시간을 줄이는 데 매우 중요합니다. 이는 텍스처 압축, 모델 복잡도 감소, 적절한 오디오 포맷 선택 등을 포함합니다.130

#### **구체적인 구현 방법**

* **텍스처:**  
  * **압축:** 플랫폼별 텍스처 압축 포맷(예: 안드로이드용 ASTC 또는 ETC2, iOS용 ASTC)을 사용하십시오.131 이는 메모리 사용량을 크게 줄입니다. 텍스처 임포터의 플랫폼별 오버라이드 설정을 활용하십시오.134  
  * **해상도:** 시각적으로 허용 가능한 가장 낮은 해상도(Max Size)를 사용하십시오. 모든 텍스처가 4K일 필요는 없습니다.135  
  * **밉맵(Mip Maps):** 카메라로부터 거리가 변하지 않는 UI 요소나 2D 스프라이트의 경우 밉맵을 비활성화하여 메모리를 절약하십시오.134  
  * **스프라이트 아틀라스(Sprite Atlases):** UI 스프라이트와 2D 에셋을 스프라이트 아틀라스로 그룹화하여 배칭을 통해 드로우 콜(draw call)을 줄이십시오.130  
* **모델:**  
  * **폴리곤 수:** 모델의 폴리곤 수를 가능한 한 줄이십시오. 고품질 모델은 주요 에셋에만 사용하고, 배경 요소는 더 단순하게 만들어야 합니다.138  
  * **LOD (Level of Detail):** LOD 그룹을 사용하여 모델이 카메라에서 멀어질수록 자동으로 폴리곤 수가 적은 버전으로 전환되도록 하십시오.138  
  * **메시 압축:** 모델의 임포트 설정에서 메시 압축을 활성화하여 파일 크기를 줄이십시오.138  
* **오디오:**  
  * **압축:** 압축된 오디오 포맷을 사용하십시오(예: 대부분의 사운드에 Vorbis, 음악에 MP3). 압축되지 않은 WAV 파일은 꼭 필요한 경우에만 사용해야 합니다.141  
  * **로드 타입:** 긴 음악 트랙의 경우, Load Type을 Streaming으로 설정하여 전체 파일을 한 번에 메모리에 로드하는 것을 피하십시오.141

### **6.3. Unity 프로파일러를 이용한 선제적 성능 분석**

Unity 프로파일러는 성능 병목 현상을 식별하는 데 필수적인 도구입니다. CPU 사용량, GPU 사용량, 메모리 할당 등에 대한 상세한 정보를 제공합니다.142 프로파일링은 마지막 순간에 하는 작업이 아니라 개발 과정 전반에 걸쳐 지속적으로 이루어져야 합니다.143

#### **구체적인 구현 방법**

1. Window \> Analysis \> Profiler를 통해 프로파일러를 엽니다.145  
2. 에디터에서 "Record" 버튼을 활성화한 상태로 게임을 실행하여 실시간 성능 데이터를 확인합니다. 더 정확한 데이터를 얻으려면 개발 빌드를 생성하여 실행 중인 애플리케이션에 프로파일러를 연결하십시오.146  
3. **CPU Usage Module:** 타임라인에서 스파이크(spike)를 찾습니다. 특정 프레임을 클릭하면 어떤 함수 호출이 가장 많은 시간을 소모하는지 계층 구조로 볼 수 있습니다.147  
4. **Memory Module:** 가비지 컬렉션(GC) 스파이크를 모니터링합니다. 이는 게임의 끊김 현상(stutter)을 유발합니다. "Simple" 뷰를 사용하여 GC Alloc을 추적하고, Update()와 같이 성능에 민감한 코드에서 발생하는 할당을 최소화하거나 제거하는 것을 목표로 합니다.  
5. **UI Details Module:** UI 프로파일링 전용 모듈입니다. 얼마나 많은 배치가 생성되고 있는지, 그리고 요소들이 왜 함께 배치되지 않는지에 대한 정보를 보여주어 UGUI 최적화에 매우 유용합니다.145

### **6.4. UI 관련 성능 고려사항 및 해결책**

UI Toolkit은 일반적으로 UGUI보다 성능이 우수하지만, 원활한 UI를 보장하기 위해 따라야 할 모범 사례들이 있습니다.

#### **구체적인 구현 방법**

* **지오메트리 재생성 최소화:** 섹션 5.3에서 언급했듯이, 레이아웃 엔진의 불필요한 작업을 피하기 위해 레이아웃 속성(width, margin 등) 대신 transform 및 opacity 속성에 애니메이션을 적용하십시오.109  
* **리스트 가상화:** 긴 아이템 목록에는 ListView 컨트롤을 사용하십시오. 이는 가상화되어 있어 현재 화면에 보이는 아이템에 대한 시각적 요소만 생성하고 사용자가 스크롤할 때 이를 재활용합니다. 이를 통해 수천 개의 아이템이 있는 목록도 원활하게 작동할 수 있습니다.150 표준  
  ScrollView에 모든 요소를 인스턴스화하여 긴 목록을 만드는 것은 흔하면서도 심각한 성능 저하를 유발합니다.  
* **이벤트 디스패칭:** Unity 6는 UI Toolkit의 이벤트 디스패칭 성능을 크게 개선하여 훨씬 빨라졌습니다.149 그럼에도 불구하고, 불필요하게 깊게 중첩된 계층 구조에서 복잡한 이벤트 버블링은 피하는 것이 좋습니다.  
* **동적 아틀라스:** UI Toolkit은 UI 요소의 텍스처를 배치하기 위해 동적 아틀라스를 사용합니다. 이 기능이 효과적으로 작동하도록 하려면 UI 아이콘과 배경에 사용되는 소스 텍스처의 수를 줄이는 것이 좋습니다.155

#### **주의점**

UI 성능의 가장 큰 함정은 종종 레이아웃 재구축입니다.157 단일 요소의 변경이 부모 레이아웃 그룹으로 하여금 모든 자식의 위치를 재계산하게 만들 수 있습니다. 자주 변경되는 UI 부분은 자체 컨테이너로 분리하여 레이아웃 재계산의 범위를 제한하는 것이 중요합니다.

잘 설계된 아키텍처는 에셋 최적화를 훨씬 쉽고 효과적으로 만듭니다. 예를 들어, ScriptableObject를 사용하여 데이터를 정의하면, 아티스트는 모델의 LOD를 만들고 디자이너는 데이터 변형을 생성하는 동안, 개발자는 이 모든 것을 동일한 고도로 최적화된 프리팹에 간단히 연결할 수 있습니다. 로직, 데이터, 시각적 표현이 모두 분리되어 있기 때문에 가능한 일입니다. 이처럼 좋은 아키텍처는 좋은 최적화를 가능하게 합니다.

## **결론**

본 보고서에서 제시된 단계별 실행 계획은 Unity와 UI Toolkit을 사용하여 견고하고 확장 가능하며 유지보수가 용이한 애플리케이션을 구축하기 위한 포괄적인 로드맵을 제공합니다. 이는 단순히 일련의 기술적 작업을 나열하는 것을 넘어, 프로젝트의 전체 수명 주기 동안 기술적 우수성을 유지하기 위한 통합된 방법론을 제시합니다.

프로젝트의 성공은 견고한 기반에서 시작됩니다. 버전 관리를 위한 Force Text 직렬화, 정확한 .gitignore 설정, 그리고 대용량 파일을 위한 Git LFS의 도입은 단순한 설정이 아니라, 미래의 병합 충돌과 저장소 문제를 예방하는 핵심적인 아키텍처 결정입니다. 마찬가지로, MVVM과 같은 현대적인 아키텍처 패턴을 채택하고, 싱글톤, 이벤트 시스템, ScriptableObject를 적절히 활용하여 시스템을 분리하는 것은 코드의 유연성과 테스트 용이성을 극대화합니다.

UI Toolkit의 강력함은 UXML, USS, C\#의 명확한 역할 분리와 Flexbox 기반의 반응형 레이아웃 시스템에서 비롯됩니다. 이는 팀원 간의 원활한 협업을 가능하게 하며, 다양한 플랫폼과 해상도에 대응할 수 있는 유연한 UI를 구축하는 기반이 됩니다. 데이터 바인딩, 커스텀 컨트롤, USS 트랜지션과 같은 고급 기능들은 UI를 단순한 정보 표시판에서 벗어나, 데이터와 유기적으로 상호작용하는 '살아있는' 시스템으로 발전시킵니다.

마지막으로, 성능 최적화는 개발 마지막 단계의 작업이 아니라, 프로파일러를 통한 지속적인 측정, 식별, 리팩토링의 순환 과정임을 인지해야 합니다. 잘 설계된 아키텍처는 에셋 통합과 최적화 과정을 단순화하며, ListView의 가상화와 같은 UI Toolkit 고유의 성능 최적화 기법을 이해하고 활용하는 것이 중요합니다.

이 계획서를 따름으로써 개발팀은 기술적 부채를 최소화하고, 변화하는 요구사항에 신속하게 대응하며, 최종적으로는 사용자에게 고품질의 경험을 제공하는 성공적인 프로젝트를 완성할 수 있을 것입니다.

#### **참고 자료**

1. Getting started with UI toolkit \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/getting-started-with-ui-toolkit](https://learn.unity.com/tutorial/getting-started-with-ui-toolkit)  
2. How to use Unity with GitHub in 2025 \- Anchorpoint, 8월 3, 2025에 액세스, [https://www.anchorpoint.app/blog/github-and-unity](https://www.anchorpoint.app/blog/github-and-unity)  
3. Put a existing Unity project into a Github repo : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/1j4edic/put\_a\_existing\_unity\_project\_into\_a\_github\_repo/](https://www.reddit.com/r/Unity3D/comments/1j4edic/put_a_existing_unity_project_into_a_github_repo/)  
4. dev.to, 8월 3, 2025에 액세스, [https://dev.to/colin-williams-dev/how-to-set-up-git-and-gh-for-unity-2198](https://dev.to/colin-williams-dev/how-to-set-up-git-and-gh-for-unity-2198)  
5. Guide: Using GitHub and Unity (From a Game Dev) \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/unity/comments/1adjewj/guide\_using\_github\_and\_unity\_from\_a\_game\_dev/](https://www.reddit.com/r/unity/comments/1adjewj/guide_using_github_and_unity_from_a_game_dev/)  
6. \[Unity\] 유니티와 깃허브(GitHub) 연동하기 1 \- Logic \- 티스토리, 8월 3, 2025에 액세스, [https://kimasill.tistory.com/entry/Unity-%EC%9C%A0%EB%8B%88%ED%8B%B0%EC%99%80-%EA%B9%83%ED%97%88%EB%B8%8CGitHub-%EC%97%B0%EB%8F%99%ED%95%98%EA%B8%B0-1](https://kimasill.tistory.com/entry/Unity-%EC%9C%A0%EB%8B%88%ED%8B%B0%EC%99%80-%EA%B9%83%ED%97%88%EB%B8%8CGitHub-%EC%97%B0%EB%8F%99%ED%95%98%EA%B8%B0-1)  
7. 유니티 GitHub 기본 연동방법, 8월 3, 2025에 액세스, [https://makerejoicegames.tistory.com/651](https://makerejoicegames.tistory.com/651)  
8. 유니티 깃허브에 연동하는 방법 feat.소스트리, 8월 3, 2025에 액세스, [https://raimbow10.tistory.com/37](https://raimbow10.tistory.com/37)  
9. How to set up a .gitignore file for Unity \- Anchorpoint, 8월 3, 2025에 액세스, [https://www.anchorpoint.app/blog/how-to-set-up-a-gitignore-file-for-unity](https://www.anchorpoint.app/blog/how-to-set-up-a-gitignore-file-for-unity)  
10. Github로 Unity 프로젝트 관리하기 \- JJukE's Brain \- 티스토리, 8월 3, 2025에 액세스, [https://jjuke-brain.tistory.com/entry/Github%EB%A1%9C-Unity-%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EA%B4%80%EB%A6%AC%ED%95%98%EA%B8%B0](https://jjuke-brain.tistory.com/entry/Github%EB%A1%9C-Unity-%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EA%B4%80%EB%A6%AC%ED%95%98%EA%B8%B0)  
11. raw.githubusercontent.com, 8월 3, 2025에 액세스, [https://raw.githubusercontent.com/github/gitignore/master/Unity.gitignore](https://raw.githubusercontent.com/github/gitignore/master/Unity.gitignore)  
12. 유니티 프로젝트 구조 이해하기 \- GameMakersLab \- 티스토리, 8월 3, 2025에 액세스, [https://gamemakerslab.tistory.com/3](https://gamemakerslab.tistory.com/3)  
13. .gitignore for Unity Projects \- Hextant Studios, 8월 3, 2025에 액세스, [https://hextantstudios.com/unity-gitignore/](https://hextantstudios.com/unity-gitignore/)  
14. Git LFS, 8월 3, 2025에 액세스, [https://git-lfs.com/](https://git-lfs.com/)  
15. How to Properly Setup Git for Unreal Engine, Unity and Godot Projects, 8월 3, 2025에 액세스, [https://alfredbaudisch.com/blog/gamedev/how-to-properly-setup-git-for-unreal-engine-unity-and-godot-projects/](https://alfredbaudisch.com/blog/gamedev/how-to-properly-setup-git-for-unreal-engine-unity-and-godot-projects/)  
16. Installing Git Large File Storage \- GitHub Docs, 8월 3, 2025에 액세스, [https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage](https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage)  
17. mikewesthad/unity-git-and-lfs: Template for Unity \+ Git \+ Git ... \- GitHub, 8월 3, 2025에 액세스, [https://github.com/mikewesthad/unity-git-and-lfs](https://github.com/mikewesthad/unity-git-and-lfs)  
18. How To Set Up Git LFS Into Your Unity Project | by Adam Reed | Medium, 8월 3, 2025에 액세스, [https://adamwreed93.medium.com/how-to-set-up-git-lfs-into-your-unity-project-9fd276305fe7](https://adamwreed93.medium.com/how-to-set-up-git-lfs-into-your-unity-project-9fd276305fe7)  
19. \[Github\] 유니티에서 100MB이상 파일 추가할 때 발생하는 문제 해결하기 / 유니티 깃허브 초기설정 / 유니티 .gitignore 설정 \- 공보다작은골대 \- 티스토리, 8월 3, 2025에 액세스, [https://zizh.tistory.com/86](https://zizh.tistory.com/86)  
20. \[Unity?\] Github LFS 설정 | 깃허브 업로드 용량 제한 해결하기 \- 개발하는 멍정이야기, 8월 3, 2025에 액세스, [https://meongjeong.tistory.com/entry/Unity-Github-LFS-%EC%84%A4%EC%A0%95-%EA%B9%83%ED%97%88%EB%B8%8C-%EC%97%85%EB%A1%9C%EB%93%9C-%EC%9A%A9%EB%9F%89-%EC%A0%9C%ED%95%9C-%ED%95%B4%EA%B2%B0%ED%95%98%EA%B8%B0](https://meongjeong.tistory.com/entry/Unity-Github-LFS-%EC%84%A4%EC%A0%95-%EA%B9%83%ED%97%88%EB%B8%8C-%EC%97%85%EB%A1%9C%EB%93%9C-%EC%9A%A9%EB%9F%89-%EC%A0%9C%ED%95%9C-%ED%95%B4%EA%B2%B0%ED%95%98%EA%B8%B0)  
21. Best practices for organizing your Unity project, 8월 3, 2025에 액세스, [https://unity.com/how-to/organizing-your-project](https://unity.com/how-to/organizing-your-project)  
22. How to structure your Unity project (best practice tips) \- Game Dev Beginner, 8월 3, 2025에 액세스, [https://gamedevbeginner.com/how-to-structure-your-unity-project-best-practice-tips/](https://gamedevbeginner.com/how-to-structure-your-unity-project-best-practice-tips/)  
23. A guide to folder structures for Unity 6 projects \- Anchorpoint, 8월 3, 2025에 액세스, [https://www.anchorpoint.app/blog/unity-folder-structure](https://www.anchorpoint.app/blog/unity-folder-structure)  
24. Unity 프로젝트를 구성하기 위한 모범 사례, 8월 3, 2025에 액세스, [https://unity.com/kr/how-to/organizing-your-project](https://unity.com/kr/how-to/organizing-your-project)  
25. Build a modular codebase with MVC and MVP programming ..., 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/build-a-modular-codebase-with-mvc-and-mvp-programming-patterns-1?uv=6\&projectId=67bc8deaedbc2a23a7389cab](https://learn.unity.com/tutorial/build-a-modular-codebase-with-mvc-and-mvp-programming-patterns-1?uv=6&projectId=67bc8deaedbc2a23a7389cab)  
26. \[Unity\] MVC, MVP, MVVM Pattern \- Patrick's Devlog \- 티스토리, 8월 3, 2025에 액세스, [https://patrick-jy.tistory.com/172](https://patrick-jy.tistory.com/172)  
27. Introduction to the MVVM Pattern | App UI | 0.3.9 \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/Packages/com.unity.dt.app-ui@0.3/manual/mvvm-intro.html](https://docs.unity3d.com/Packages/com.unity.dt.app-ui@0.3/manual/mvvm-intro.html)  
28. 유니티 MVC(Model \- View- Controller) 패턴 \- 초보 개발자의 성장 \- 티스토리, 8월 3, 2025에 액세스, [https://ljsgrowingup.tistory.com/56](https://ljsgrowingup.tistory.com/56)  
29. Improve Your Unity Code with MVC/MVP Architectural Patterns \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=v2c589RaiwY](https://www.youtube.com/watch?v=v2c589RaiwY)  
30. Exploring UI Design Patterns: MVC, MVP, and MVVM \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=tm\_paZsPsrI](https://www.youtube.com/watch?v=tm_paZsPsrI)  
31. \[유니티 TIPS\] UI에 걸맞는 MVC, MVP, MVVM 패턴 | 프로그래밍 디자인패턴, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=fxlYxhhf83s](https://www.youtube.com/watch?v=fxlYxhhf83s)  
32. 그래픽 사용자 인터페이스 개발 | Unity UI 툴킷, 8월 3, 2025에 액세스, [https://unity.com/kr/features/ui-toolkit](https://unity.com/kr/features/ui-toolkit)  
33. 코드가 필요없는 Unity 6 UI Toolkit 데이터바인딩 소개 \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=gEem0ZHFDVs](https://www.youtube.com/watch?v=gEem0ZHFDVs)  
34. Singletons in Unity (done right) \- Game Dev Beginner, 8월 3, 2025에 액세스, [https://gamedevbeginner.com/singletons-in-unity-the-right-way/](https://gamedevbeginner.com/singletons-in-unity-the-right-way/)  
35. Day 127 of Unity Dev: Singletons— Unity/C\#\! | by Ethan Martin | Dev Genius, 8월 3, 2025에 액세스, [https://blog.devgenius.io/day-127-of-unity-dev-singletons-unity-c-781e8c34adf0](https://blog.devgenius.io/day-127-of-unity-dev-singletons-unity-c-781e8c34adf0)  
36. UnityCommunity/UnitySingleton: The best way to implement singleton pattern in Unity. \- GitHub, 8월 3, 2025에 액세스, [https://github.com/UnityCommunity/UnitySingleton](https://github.com/UnityCommunity/UnitySingleton)  
37. Setting up the Game Manager using the Singleton pattern\! | by Matej Marek \- Medium, 8월 3, 2025에 액세스, [https://matej-marek94.medium.com/setting-up-the-game-manager-using-the-singleton-pattern-d46f9fcb153f](https://matej-marek94.medium.com/setting-up-the-game-manager-using-the-singleton-pattern-d46f9fcb153f)  
38. 유니티44\_게임 제작 과정 13\_게임 매니저 \- 취하게코, 8월 3, 2025에 액세스, [https://fiftiesstudy.tistory.com/229](https://fiftiesstudy.tistory.com/229)  
39. Unity Chapter 4-6. 소코반 게임 만들기 : 게임 매니저, 승리 UI, 최종 빌드 \- 평생 공부 블로그, 8월 3, 2025에 액세스, [https://ansohxxn.github.io/unity%20lesson%201/chapter4-6/](https://ansohxxn.github.io/unity%20lesson%201/chapter4-6/)  
40. Singleton — UI Manager. In Unity | by Kenny McLachlan \- Medium, 8월 3, 2025에 액세스, [https://medium.com/@kennethmclachlan11/singleton-ui-manager-92f67099a3ae](https://medium.com/@kennethmclachlan11/singleton-ui-manager-92f67099a3ae)  
41. 22.02.09 유저 인터페이스 완성하기, 8월 3, 2025에 액세스, [https://rivergembig-gameprogramming.tistory.com/55](https://rivergembig-gameprogramming.tistory.com/55)  
42. 9.4.x 게임UI를 관리하는 스크립트 만들기 \- 유니티, 8월 3, 2025에 액세스, [https://uniti.tistory.com/174](https://uniti.tistory.com/174)  
43. Create modular and maintainable code with the observer pattern \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/create-modular-and-maintainable-code-with-the-observer-pattern?uv=6\&projectId=67bc8deaedbc2a23a7389cab](https://learn.unity.com/tutorial/create-modular-and-maintainable-code-with-the-observer-pattern?uv=6&projectId=67bc8deaedbc2a23a7389cab)  
44. The Observer Pattern: Design Patterns in Unity C\# | Unity Coder Corner \- Medium, 8월 3, 2025에 액세스, [https://medium.com/unity-coder-corner/unity-the-observer-pattern-767ac65ed7bb](https://medium.com/unity-coder-corner/unity-the-observer-pattern-767ac65ed7bb)  
45. 유니티 쉽게 배우는 디자인 패턴 \- 옵저버 패턴 / Unity Design Pattern, 8월 3, 2025에 액세스, [https://goranitv.tistory.com/18](https://goranitv.tistory.com/18)  
46. \[unity\] Action vs UnityEvent \- Devartrio, 8월 3, 2025에 액세스, [https://devartrio.medium.com/unity-action-vs-unityevent-f2bdc2478d4f?source=rss------csharp-5](https://devartrio.medium.com/unity-action-vs-unityevent-f2bdc2478d4f?source=rss------csharp-5)  
47. Unity Events vs C\# Actions : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/1jdtjuz/unity\_events\_vs\_c\_actions/](https://www.reddit.com/r/Unity3D/comments/1jdtjuz/unity_events_vs_c_actions/)  
48. Event Performance: C\# vs. UnityEvent \- JacksonDunstan.com, 8월 3, 2025에 액세스, [https://www.jacksondunstan.com/articles/3335](https://www.jacksondunstan.com/articles/3335)  
49. Delegate Events VS UnityEvent, which one is superior? (If you don't know what UnityEvents are, you should probably read this. It's going to change your life.) : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/35oekm/delegate\_events\_vs\_unityevent\_which\_one\_is/](https://www.reddit.com/r/Unity3D/comments/35oekm/delegate_events_vs_unityevent_which_one_is/)  
50. Any advice for clean code? : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/1b1ux37/any\_advice\_for\_clean\_code/](https://www.reddit.com/r/Unity3D/comments/1b1ux37/any_advice_for_clean_code/)  
51. Architect your code for efficient changes and debugging with ScriptableObjects | Unity, 8월 3, 2025에 액세스, [https://unity.com/how-to/architect-game-code-scriptable-objects](https://unity.com/how-to/architect-game-code-scriptable-objects)  
52. ScriptableObject \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/class-ScriptableObject.html](https://docs.unity3d.com/6000.1/Documentation/Manual/class-ScriptableObject.html)  
53. Separate Game Data and Logic with ScriptableObjects \- Unity, 8월 3, 2025에 액세스, [https://unity.com/how-to/separate-game-data-logic-scriptable-objects](https://unity.com/how-to/separate-game-data-logic-scriptable-objects)  
54. New Game Settings as Scriptable Objects in Unity \- GameDev 2023 \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=U-aaZAmW2gY](https://www.youtube.com/watch?v=U-aaZAmW2gY)  
55. What's the right way of using Scriptable Objects? : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/1bm8dy4/whats\_the\_right\_way\_of\_using\_scriptable\_objects/](https://www.reddit.com/r/Unity3D/comments/1bm8dy4/whats_the_right_way_of_using_scriptable_objects/)  
56. Create your greybox prototype \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/pathway/creative-core/unit/prototyping/tutorial/create-your-greybox-prototype-3](https://learn.unity.com/pathway/creative-core/unit/prototyping/tutorial/create-your-greybox-prototype-3)  
57. Lab 2 \- New Project with Primitives \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/pathway/junior-programmer/unit/basic-gameplay/tutorial/67559e90edbc2a0eaf59bd44?version=](https://learn.unity.com/pathway/junior-programmer/unit/basic-gameplay/tutorial/67559e90edbc2a0eaf59bd44?version)  
58. 레벨 디자이너를 위한 최초의 전자책이 여기 있습니다. \- Unity, 8월 3, 2025에 액세스, [https://unity.com/kr/blog/games/e-book-for-level-designers](https://unity.com/kr/blog/games/e-book-for-level-designers)  
59. Tips for 1st Grayboxing Project in Unity : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/iftdug/tips\_for\_1st\_grayboxing\_project\_in\_unity/](https://www.reddit.com/r/Unity3D/comments/iftdug/tips_for_1st_grayboxing_project_in_unity/)  
60. 게임기획크리틱 13주차 \- 레벨디자인과 그레이박싱 \- se.jeon, 8월 3, 2025에 액세스, [https://dev-sieun.tistory.com/107](https://dev-sieun.tistory.com/107)  
61. Primitive and Placeholder Objects \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/530/Documentation/Manual/PrimitiveObjects.html](https://docs.unity3d.com/530/Documentation/Manual/PrimitiveObjects.html)  
62. Our first-ever e-book for level designers is here \- Unity, 8월 3, 2025에 액세스, [https://unity.com/blog/games/e-book-for-level-designers](https://unity.com/blog/games/e-book-for-level-designers)  
63. Quick start guide | Input System | 1.0.2 \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html)  
64. Actions | Input System | 1.0.2 \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Actions.html)  
65. \[Unity\] New Input System \#1 | 세팅하기 \- DIBRARY \- 티스토리, 8월 3, 2025에 액세스, [https://daekyoulibrary.tistory.com/entry/Unity-New-Input-System-1](https://daekyoulibrary.tistory.com/entry/Unity-New-Input-System-1)  
66. Setting up the Input System in Unity 6, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/setting-up-the-input-system?version=6.0](https://learn.unity.com/tutorial/setting-up-the-input-system?version=6.0)  
67. Unity's New Input System (+ How To Use It\!) \- Zero To Mastery, 8월 3, 2025에 액세스, [https://zerotomastery.io/blog/unity-new-input-system/](https://zerotomastery.io/blog/unity-new-input-system/)  
68. Unity) Input System의 사용법, 장단점, 차이점. (입력 받기) \- UniCoti \- 티스토리, 8월 3, 2025에 액세스, [https://alpaca-code.tistory.com/225](https://alpaca-code.tistory.com/225)  
69. \[Unity\] New Input System \#1 새로운 인력 시스템 사용하기 \- 공부 하자, 8월 3, 2025에 액세스, [https://hyeonjunje.github.io/unity/Unity\_NewInputSystem/](https://hyeonjunje.github.io/unity/Unity_NewInputSystem/)  
70. \[Input System\] 새로운 Input System \- 유니티 \- 티스토리, 8월 3, 2025에 액세스, [https://uniti.tistory.com/91](https://uniti.tistory.com/91)  
71. \[개념 콕\] 유니티 New Input System \- 내일배움캠프 블로그, 8월 3, 2025에 액세스, [https://nbcamp.spartacodingclub.kr/blog/%EA%B0%9C%EB%85%90-%EC%BD%95-%EC%9C%A0%EB%8B%88%ED%8B%B0-new-input-system-21545](https://nbcamp.spartacodingclub.kr/blog/%EA%B0%9C%EB%85%90-%EC%BD%95-%EC%9C%A0%EB%8B%88%ED%8B%B0-new-input-system-21545)  
72. How do YOU use Unity's new Input System? \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/unity/comments/1ftm3ij/how\_do\_you\_use\_unitys\_new\_input\_system/](https://www.reddit.com/r/unity/comments/1ftm3ij/how_do_you_use_unitys_new_input_system/)  
73. UI Toolkit Fundamentals \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/course/ui-toolkit-fundamentals](https://learn.unity.com/course/ui-toolkit-fundamentals)  
74. Unity UIToolkit 연구 1 : 기본 사용법 \- 맨텀 \- 티스토리, 8월 3, 2025에 액세스, [https://mentum.tistory.com/665](https://mentum.tistory.com/665)  
75. Unity MVVM 패턴 \- 스니커즈 정리공간 \- 티스토리, 8월 3, 2025에 액세스, [https://snikuz.tistory.com/140](https://snikuz.tistory.com/140)  
76. Get started with UI Toolkit \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-simple-ui-toolkit-workflow.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-simple-ui-toolkit-workflow.html)  
77. Structure UI with UXML \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-UXML.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-UXML.html)  
78. UI Toolkit at runtime: Get the breakdown \- Unity, 8월 3, 2025에 액세스, [https://unity.com/blog/engine-platform/ui-toolkit-at-runtime-get-the-breakdown](https://unity.com/blog/engine-platform/ui-toolkit-at-runtime-get-the-breakdown)  
79. UI Toolkit \- First steps \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/ui-toolkit-first-steps](https://learn.unity.com/tutorial/ui-toolkit-first-steps)  
80. Best practices for USS \- Unity User Manual 2021.3 (LTS), 8월 3, 2025에 액세스, [https://docs.unity.cn/Manual/UIE-USS-WritingStyleSheets.html](https://docs.unity.cn/Manual/UIE-USS-WritingStyleSheets.html)  
81. Introduction to USS \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-about-uss.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-about-uss.html)  
82. 유니티3D의 UI Toolkit (1) \- 기본세팅과 '배경 창' 꾸미기, 8월 3, 2025에 액세스, [https://itadventure.tistory.com/632](https://itadventure.tistory.com/632)  
83. UI Builder \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIBuilder.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIBuilder.html)  
84. UI 빌더 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2021.3/Manual/UIBuilder.html](https://docs.unity3d.com/kr/2021.3/Manual/UIBuilder.html)  
85. \[UI Toolkit\] 1\. UI Builder를 사용한 UI 배치 \- RYULAB \- 티스토리, 8월 3, 2025에 액세스, [https://taeyeokim.tistory.com/60](https://taeyeokim.tistory.com/60)  
86. Get started with UI Builder \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIB-getting-started.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIB-getting-started.html)  
87. UI 빌더로 시작하기 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2023.1/Manual/UIB-getting-started.html](https://docs.unity3d.com/kr/2023.1/Manual/UIB-getting-started.html)  
88. unity \- UI Toolkit uss 사용하기 \- 도동네 개발 농장 \- 티스토리, 8월 3, 2025에 액세스, [https://dodongs-development-farm.tistory.com/21](https://dodongs-development-farm.tistory.com/21)  
89. Position element with the layout engine \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-LayoutEngine.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-LayoutEngine.html)  
90. 요소 위치 지정 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2022.1/Manual/UIB-styling-ui-positioning.html](https://docs.unity3d.com/kr/2022.1/Manual/UIB-styling-ui-positioning.html)  
91. 레이아웃 엔진으로 요소 위치 지정 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2023.2/Manual/UIE-LayoutEngine.html](https://docs.unity3d.com/kr/2023.2/Manual/UIE-LayoutEngine.html)  
92. Ultimate Guide to Flexbox in Unity UI Toolkit . \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=9fh6Ej3I2JU](https://www.youtube.com/watch?v=9fh6Ej3I2JU)  
93. Find visual elements with UQuery \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/Manual/UIE-UQuery.html](https://docs.unity3d.com/Manual/UIE-UQuery.html)  
94. 유니티3D의 UI Toolkit (4) \- 버튼 클릭 이벤트 \- 크레이의 IT탐구 \- 티스토리, 8월 3, 2025에 액세스, [https://itadventure.tistory.com/643](https://itadventure.tistory.com/643)  
95. Find visual elements with UQuery \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity.cn/kr/2022.1/Manual/UIE-UQuery.html](https://docs.unity.cn/kr/2022.1/Manual/UIE-UQuery.html)  
96. Click events \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-Click-Events.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-Click-Events.html)  
97. Manual: Create a simple transition with UI Builder and C\# scripts, 8월 3, 2025에 액세스, [https://docs.unity.cn/Manual/UIE-transition-example.html](https://docs.unity.cn/Manual/UIE-transition-example.html)  
98. Data binding \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-data-binding.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-data-binding.html)  
99. Data binding \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/2023.2/Documentation/Manual/UIE-data-binding.html](https://docs.unity3d.com/2023.2/Documentation/Manual/UIE-data-binding.html)  
100. Runtime UI Data Binding with UI Toolkit | Unity Tutorial \- YouTube, 8월 3, 2025에 액세스, [https://m.youtube.com/watch?v=\_FlgT0bB\_pY](https://m.youtube.com/watch?v=_FlgT0bB_pY)  
101. UI 툴킷 시작하기 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2023.2/Manual/UIE-simple-ui-toolkit-workflow.html](https://docs.unity3d.com/kr/2023.2/Manual/UIE-simple-ui-toolkit-workflow.html)  
102. Part 3: Unity UI Toolkit: MainMenu, Databinding with ScriptableObject \- public gitlab repository \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=5CsN3y9Jduo](https://www.youtube.com/watch?v=5CsN3y9Jduo)  
103. Create a custom control \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-create-custom-controls.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-create-custom-controls.html)  
104. UI Toolkit Tutorial \- Custom Components \- Switch \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=v5wveVhVEUA](https://www.youtube.com/watch?v=v5wveVhVEUA)  
105. Custom controls \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/2022.1/Documentation/Manual/UIE-create-custom-controls.html](https://docs.unity3d.com/2022.1/Documentation/Manual/UIE-create-custom-controls.html)  
106. 두 가지 속성을 사용하여 커스텀 컨트롤 만들기 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity.cn/kr//Manual/UIB-structuring-ui-custom-elements.html](https://docs.unity.cn/kr//Manual/UIB-structuring-ui-custom-elements.html)  
107. 커스텀 컨트롤 만들기 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2023.1/Manual/UIE-create-custom-controls.html](https://docs.unity3d.com/kr/2023.1/Manual/UIE-create-custom-controls.html)  
108. Develop Graphical User Interfaces | Unity UI Toolkit, 8월 3, 2025에 액세스, [https://unity.com/features/ui-toolkit](https://unity.com/features/ui-toolkit)  
109. USS transition \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-Transitions.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-Transitions.html)  
110. Get started with UI Toolkit in Unity \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=\_jtj73lu2Ko](https://www.youtube.com/watch?v=_jtj73lu2Ko)  
111. 레이아웃과 버튼 애니메이션 \[유니티 UI 강의, Unity UI Toolkit 강좌\] \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=eeDjeziVEbA](https://www.youtube.com/watch?v=eeDjeziVEbA)  
112. From Bland to Engaging: Make Your UI Toolkit UI Pop with Animations | Unity Tutorial, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=FhIvEETXr78](https://www.youtube.com/watch?v=FhIvEETXr78)  
113. How To Animate In UI TOOLKIT || Unity \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=qm59GPmNtek](https://www.youtube.com/watch?v=qm59GPmNtek)  
114. UI toolkit transit animation on turning on vibility? : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/z881et/ui\_toolkit\_transit\_animation\_on\_turning\_on/](https://www.reddit.com/r/Unity3D/comments/z881et/ui_toolkit_transit_animation_on_turning_on/)  
115. New UI Toolkit demos for programmers and artists | Unity Blog, 8월 3, 2025에 액세스, [https://unity.com/blog/engine-platform/new-ui-toolkit-demos-for-programmers-artists](https://unity.com/blog/engine-platform/new-ui-toolkit-demos-for-programmers-artists)  
116. My disappointment with Unity's UI Toolkit \- Musing Mortoray, 8월 3, 2025에 액세스, [https://mortoray.com/my-disappointment-with-unity-uitoolkit/](https://mortoray.com/my-disappointment-with-unity-uitoolkit/)  
117. How can I make a responsive ui with ui toolkit? : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/z8b8de/how\_can\_i\_make\_a\_responsive\_ui\_with\_ui\_toolkit/](https://www.reddit.com/r/Unity3D/comments/z8b8de/how_can_i_make_a_responsive_ui_with_ui_toolkit/)  
118. How to EASILY Create a Responsive UI in Unity | Unity Tutorial \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=5IDGgg1xsgQ](https://www.youtube.com/watch?v=5IDGgg1xsgQ)  
119. \[Unity Tip\] 유니티 반응형 UI 설정하기 (Unity responsive UI setting) \- iOEEO's \- 티스토리, 8월 3, 2025에 액세스, [https://dydvn.tistory.com/21](https://dydvn.tistory.com/21)  
120. \[Unity\] 반응형 UI를 제작할 때 필요한 배경지식 \- 하늘서랍, 8월 3, 2025에 액세스, [https://artiper.tistory.com/182](https://artiper.tistory.com/182)  
121. 게임 UI 해상도 대응을 위한 2가지 세팅\! (feat.유니티 UI) \- 셈디자인클래스, 8월 3, 2025에 액세스, [https://www.semdesignclass.com/blog/gameui-screen-resolution-2](https://www.semdesignclass.com/blog/gameui-screen-resolution-2)  
122. \[Unity\] 해상도에 따른 화면 비율 유지(feat. Canvas Scaler) \- 극꼼이 이야기 (GG\_Tales), 8월 3, 2025에 액세스, [https://geukggom.tistory.com/129](https://geukggom.tistory.com/129)  
123. Designing UI for Multiple Resolutions \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/510/Documentation/Manual/HOWTO-UIMultiResolution.html](https://docs.unity3d.com/510/Documentation/Manual/HOWTO-UIMultiResolution.html)  
124. Is there a be-all, end-all explanation/tutorial on making UI work on all mobile screen resolutions? : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/16zkg9n/is\_there\_a\_beall\_endall\_explanationtutorial\_on/](https://www.reddit.com/r/Unity3D/comments/16zkg9n/is_there_a_beall_endall_explanationtutorial_on/)  
125. 유니티 모바일 시뮬레이터로 다양한 해상도 대응하기 / Respond to Wide Range of Resolutions with Unity Mobile Simulator, 8월 3, 2025에 액세스, [https://goranitv.tistory.com/11](https://goranitv.tistory.com/11)  
126. Introduction to Asset Manager Transfer Methods \- Unity, 8월 3, 2025에 액세스, [https://unity.com/resources/asset-manager-transfer-methods](https://unity.com/resources/asset-manager-transfer-methods)  
127. How to Use Unity Asset Manager, 8월 3, 2025에 액세스, [https://unity.com/resources/how-to-use-unity-asset-manager](https://unity.com/resources/how-to-use-unity-asset-manager)  
128. Unity Asset Manager를 사용하는 방법, 8월 3, 2025에 액세스, [https://unity.com/kr/resources/how-to-use-unity-asset-manager](https://unity.com/kr/resources/how-to-use-unity-asset-manager)  
129. Art Asset best practice guide \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/2020.1/Documentation/Manual/HOWTO-ArtAssetBestPracticeGuide.html](https://docs.unity3d.com/2020.1/Documentation/Manual/HOWTO-ArtAssetBestPracticeGuide.html)  
130. Introduction to Optimization in Unity \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/introduction-to-optimization-in-unity](https://learn.unity.com/tutorial/introduction-to-optimization-in-unity)  
131. Unity and ASTC \- Adaptive Scalable Texture Compression Developer Guide, 8월 3, 2025에 액세스, [https://developer.arm.com/documentation/102162/latest/Unity-and-ASTC](https://developer.arm.com/documentation/102162/latest/Unity-and-ASTC)  
132. An Introduction to Texture Compression in Unity \- techarthub, 8월 3, 2025에 액세스, [https://techarthub.com/an-introduction-to-texture-compression-in-unity/](https://techarthub.com/an-introduction-to-texture-compression-in-unity/)  
133. Target texture compression formats in Android App Bundles | Other Play guides, 8월 3, 2025에 액세스, [https://developer.android.com/guide/playcore/asset-delivery/texture-compression](https://developer.android.com/guide/playcore/asset-delivery/texture-compression)  
134. Configuring your Unity project for stronger performance, 8월 3, 2025에 액세스, [https://unity.com/how-to/project-configuration-and-assets](https://unity.com/how-to/project-configuration-and-assets)  
135. Optimize your mobile game performance: Expert tips on graphics and assets \- Unity, 8월 3, 2025에 액세스, [https://unity.com/blog/games/optimize-your-mobile-game-performance-expert-tips-on-graphics-and-assets](https://unity.com/blog/games/optimize-your-mobile-game-performance-expert-tips-on-graphics-and-assets)  
136. Art optimization tips for mobile game developers part 1 \- Unity, 8월 3, 2025에 액세스, [https://unity.com/how-to/mobile-game-optimization-tips-part-1](https://unity.com/how-to/mobile-game-optimization-tips-part-1)  
137. Unity UI Optimization Workflow: Step-by-Step full guide for everyone : r/unity\_tutorials, 8월 3, 2025에 액세스, [https://www.reddit.com/r/unity\_tutorials/comments/1bkwebv/unity\_ui\_optimization\_workflow\_stepbystep\_full/](https://www.reddit.com/r/unity_tutorials/comments/1bkwebv/unity_ui_optimization_workflow_stepbystep_full/)  
138. Unity optimization techniques : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/19datci/unity\_optimization\_techniques/](https://www.reddit.com/r/Unity3D/comments/19datci/unity_optimization_techniques/)  
139. Optimizing graphics performance \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/2017.4/Documentation/Manual/OptimizingGraphicsPerformance.html](https://docs.unity3d.com/2017.4/Documentation/Manual/OptimizingGraphicsPerformance.html)  
140. 모바일 게임 개발자를 위한 아트 최적화 팁 1부 \- Unity, 8월 3, 2025에 액세스, [https://unity.com/kr/how-to/mobile-game-optimization-tips-part-1](https://unity.com/kr/how-to/mobile-game-optimization-tips-part-1)  
141. Optimization Tips to Boost Performance in Unity \- Vagon, 8월 3, 2025에 액세스, [https://vagon.io/blog/optimization-tips-for-unity](https://vagon.io/blog/optimization-tips-for-unity)  
142. Unity Profiler \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/Profiler.html](https://docs.unity3d.com/6000.1/Documentation/Manual/Profiler.html)  
143. Profiling and debugging with Unity and native platform tools, 8월 3, 2025에 액세스, [https://unity.com/how-to/profiling-and-debugging-tools](https://unity.com/how-to/profiling-and-debugging-tools)  
144. Unity Profiler로 성능 지표 커스터마이즈하기, 8월 3, 2025에 액세스, [https://unity.com/kr/blog/engine-platform/customizing-performance-metrics-in-the-unity-profiler](https://unity.com/kr/blog/engine-platform/customizing-performance-metrics-in-the-unity-profiler)  
145. UI Profiler \- Unity Learn, 8월 3, 2025에 액세스, [https://learn.unity.com/tutorial/ui-profiler-2019-3](https://learn.unity.com/tutorial/ui-profiler-2019-3)  
146. Unity Profiler Walkthrough & Tutorial \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=xjsqv8nj0cw](https://www.youtube.com/watch?v=xjsqv8nj0cw)  
147. 유니티 프로파일러(Profiler)를 이용해 성능 개선하기 \- 나는 뉴비다 개발자편, 8월 3, 2025에 액세스, [https://dev-nicitis.tistory.com/7](https://dev-nicitis.tistory.com/7)  
148. UI 및 UI 세부 정보 프로파일러 \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2021.1/Manual/ProfilerUI.html](https://docs.unity3d.com/kr/2021.1/Manual/ProfilerUI.html)  
149. Unity 6 UI Toolkit: News and Updates, 8월 3, 2025에 액세스, [https://unity.com/blog/unity-6-ui-toolkit-updates](https://unity.com/blog/unity-6-ui-toolkit-updates)  
150. Create a list view runtime UI \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-HowTo-CreateRuntimeUI.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-HowTo-CreateRuntimeUI.html)  
151. List View \- Unity Editor Foundations, 8월 3, 2025에 액세스, [https://www.foundations.unity.com/components/list-view](https://www.foundations.unity.com/components/list-view)  
152. ListView \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-uxml-element-ListView.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-uxml-element-ListView.html)  
153. How to make a List in UI Toolkit : r/Unity3D \- Reddit, 8월 3, 2025에 액세스, [https://www.reddit.com/r/Unity3D/comments/11fcjpd/how\_to\_make\_a\_list\_in\_ui\_toolkit/](https://www.reddit.com/r/Unity3D/comments/11fcjpd/how_to_make_a_list_in_ui_toolkit/)  
154. UXML 요소 ListView \- Unity 매뉴얼, 8월 3, 2025에 액세스, [https://docs.unity3d.com/kr/2022.2/Manual/UIE-uxml-element-ListView.html](https://docs.unity3d.com/kr/2022.2/Manual/UIE-uxml-element-ListView.html)  
155. Performance consideration for runtime UI \- Unity \- Manual, 8월 3, 2025에 액세스, [https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-performance-consideration-runtime.html](https://docs.unity3d.com/6000.1/Documentation/Manual/UIE-performance-consideration-runtime.html)  
156. Getting the best performance with UI Toolkit | Unite 2024 \- YouTube, 8월 3, 2025에 액세스, [https://www.youtube.com/watch?v=bECmaYIvZJg](https://www.youtube.com/watch?v=bECmaYIvZJg)  
157. How to optimize UIs in Unity: slow performance causes and solutions \- Medium, 8월 3, 2025에 액세스, [https://medium.com/my-games-company/how-to-optimize-uis-in-unity-slow-performance-causes-and-solutions-c47af453b1db](https://medium.com/my-games-company/how-to-optimize-uis-in-unity-slow-performance-causes-and-solutions-c47af453b1db)  
158. Optimizing UI Performance in Unity: Deep Dive into LayoutElement and LayoutGroup Components | by magic Hung, 8월 3, 2025에 액세스, [https://llmagicll.medium.com/optimizing-ui-performance-in-unity-deep-dive-into-layoutelement-and-layoutgroup-components-b6a575187ee4](https://llmagicll.medium.com/optimizing-ui-performance-in-unity-deep-dive-into-layoutelement-and-layoutgroup-components-b6a575187ee4)